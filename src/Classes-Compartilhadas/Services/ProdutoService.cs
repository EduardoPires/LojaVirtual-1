using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classes_Compartilhadas.Services
{
    public class ProdutoService
    {
        private readonly ApplicationDbContext _context;

        public ProdutoService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Produto>> TodosProdutos()
        {
            return await _context.Produto.Include(p => p.Categoria).Include(p => p.Vendedor).ToListAsync();
        }

        public async Task<bool> ProdutoExiste(int id)
        {
            return await _context.Produto.AnyAsync(c => c.Id == id);
        }


        public async Task<bool> ProdutoPodeSerExcluido(int id, string id_vendedor)
        {
            return await _context.Produto.AnyAsync(p => p.Id == id && p.VendedorId == id_vendedor);
        }





        public async Task<Produto?> ObterProdutoPorId(int? id)
        {
            return await _context.Produto.Include(p => p.Categoria).Include(p => p.Vendedor).FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<bool> TemProduto()
        {
            if (_context.Produto == null)
                return false;

            return true;
        }

        public async Task InserirProduto(Produto produto)
        {
            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

        }

        public async Task AtualizarProduto(Produto produto)
        {
            _context.Produto.Update(produto);
            await _context.SaveChangesAsync();

        }

        public async Task RemoverProduto(Produto produto)
        {
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
        }




        public async Task<string> AtualizaImagemAsync(IFormFile imagem, string nomeAnterior)
        {
            if (imagem == null || imagem.Length == 0)
                throw new ArgumentException("A imagem enviada é inválida.");

            var raiz = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            if (string.IsNullOrEmpty(raiz))
                throw new DirectoryNotFoundException("Não foi possível localizar a raiz do projeto.");

            
            var caminhoProjetoMvc = Path.Combine(raiz, "LojaVirtual", "src", "LojaVirtualMvc", "wwwroot", "images");

            if (!Directory.Exists(caminhoProjetoMvc))
                Directory.CreateDirectory(caminhoProjetoMvc);

            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            var caminhoCompleto = Path.Combine(caminhoProjetoMvc, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            // Remove imagem anterior
            if (!string.IsNullOrEmpty(nomeAnterior))
            {
                var nomeArquivoAnterior = Path.GetFileName(nomeAnterior);
                var caminhoAntigo = Path.Combine(caminhoProjetoMvc, nomeArquivoAnterior);

                if (System.IO.File.Exists(caminhoAntigo))
                {
                    System.IO.File.Delete(caminhoAntigo);
                }
            }

            return "/images/" + nomeArquivo;
        }


        public async Task<string> InserirImagemAsync(IFormFile imagem)
        {
            var raiz = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var caminhoProjetoMvc = Path.Combine(raiz, "LojaVirtual", "src", "LojaVirtualMvc", "wwwroot", "images");
            var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);

            if (!Directory.Exists(caminhoProjetoMvc))
                Directory.CreateDirectory(caminhoProjetoMvc);

            var caminhoCompleto = Path.Combine(caminhoProjetoMvc, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            return "/images/" + nomeArquivo;
        }

















    }
}
