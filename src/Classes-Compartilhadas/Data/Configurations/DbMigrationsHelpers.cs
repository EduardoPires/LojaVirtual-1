using Classes_Compartilhadas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Classes_Compartilhadas.Data.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateAsyncScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedCategories(ApplicationDbContext context)
        {
            if (context.Categoria.Any()) return;

            var categorias = new[]
            {
                new Categoria { Nome = "Carros", Descricao = "Veículos de passeio" },
                new Categoria { Nome = "Motos", Descricao = "Motocicletas de todos os tipos" },
                new Categoria { Nome = "Peças", Descricao = "Peças de veículos" },
                new Categoria { Nome = "Casa e Decoração", Descricao = "Itens para deixar sua casa mais aconchegante" },
                new Categoria { Nome = "Alimentos e Bebidas", Descricao = "Delícias e bebidas para todos os gostos." },
            };

            await context.Categoria.AddRangeAsync(categorias);
            await context.SaveChangesAsync();
        }

        public static async Task EnsureSeedVendors(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.Vendedor.Any()) return;
            var user = new IdentityUser
            {
                UserName = "vendedor", 
                Email = "vendas@gmail.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Teste@123");

            if (!result.Succeeded)
            {
                throw new Exception("Erro ao criar o usuário vendedor");
            }

            var vendedor = new Vendedor
            {
                Id = user.Id, 
                Nome = "Carlos Oliveira",
                DataNascimento = new DateTime(1990, 5, 10)
            };

            context.Vendedor.Add(vendedor);
            await context.SaveChangesAsync();
        }


        public static async Task EnsureSeedProducts(ApplicationDbContext context)
        {
            if (context.Produto.Any()) return;

            var categoria = context.Categoria.First();
            var vendedor = context.Vendedor.First();

            var produtos = new[]
            {
                new Produto
                {
                    Descricao = "CB 1000",
                    Imagem = "/images/cb_1000.jpg",
                    Preco = 45000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 2,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "D 20",
                    Imagem = "/images/d20.jpg",
                    Preco = 85000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = categoria.Id,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "HARLEY ",
                    Imagem = "/images/harley.jpg",
                    Preco = 120000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 2,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "OPALA COUPE ",
                    Imagem = "/images/opala.jpg",
                    Preco = 120000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "OPALA COUPE TURBO",
                    Imagem = "/images/opala2.jpg",
                    Preco = 150000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "SILVERADO",
                    Imagem = "/images/silverado.jpg",
                    Preco = 320000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "VW VOYAGE",
                    Imagem = "/images/voyage1.jpg",
                    Preco = 35000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "VW VOYAGE",
                    Imagem = "/images/voyage2.jpg",
                    Preco = 23000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                },
                new Produto
                {
                    Descricao = "VW VOYAGE",
                    Imagem = "/images/voyage3.jpg",
                    Preco = 20000.00,
                    Estoque = 1,
                    DataAnuncio = DateTime.Now,
                    CategoriaId = 1,
                    VendedorId = vendedor.Id
                }
            };

            await context.Produto.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
            {
                await context.Database.MigrateAsync();

                await EnsureSeedCategories(context);

                await EnsureSeedVendors(context, userManager);

                await EnsureSeedProducts(context);
            }
        }
    }
}
