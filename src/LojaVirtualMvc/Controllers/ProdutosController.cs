using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Classes_Compartilhadas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtualMvc.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ProdutoService _produtoService;

        public ProdutosController(ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            ProdutoService produtoService)
        {
            _context = context;
            _signInManager = signInManager;
            _produtoService = produtoService;
        }



        [AllowAnonymous]
        // GET: Produtoes
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoService.TodosProdutos();
            return View(produtos);
        }

        public async Task<IActionResult> MyProducts()
        {
            string userId = _signInManager.UserManager.GetUserId(User);
            var applicationDbContext = _context.Produto.Include(p => p.Categoria).Include(p => p.Vendedor).Where(p => p.VendedorId == userId);
            return View(await applicationDbContext.ToListAsync());
        }


        [AllowAnonymous]
        // GET: Produtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _produtoService.ObterProdutoPorId(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }


        // GET: Produtoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome");

            return View();
        }

        // POST: Produtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,ImagemUpload,Preco,Estoque,CategoriaId")] Produto produto)
        {
            
            string userId = _signInManager.UserManager.GetUserId(User);
            produto.VendedorId = userId;
            produto.Imagem = "z";
            

            ModelState.Clear();
            TryValidateModel(produto);

            if (ModelState.IsValid)
            {
                produto.Imagem = await _produtoService.InserirImagemAsync(produto.ImagemUpload);
                _produtoService.InserirProduto(produto);

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }





        // GET: Produtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoService.ObterProdutoPorId(id);

            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", produto.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Set<Vendedor>(), "Id", "Id", produto.VendedorId);
            return View(produto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,ImagemUpload,Imagem,Preco,Estoque,CategoriaId, VendedorId")] Produto produto)
        {
            if (id != produto.Id)
                return NotFound();



            if (ModelState.IsValid)
            {
                try
                {
                    if(produto.ImagemUpload != null)
                    {
                        produto.Imagem = await _produtoService.AtualizaImagemAsync(produto.ImagemUpload, produto.Imagem);
                    }
                    _produtoService.AtualizarProduto(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", produto.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Set<Vendedor>(), "Id", "Id", produto.VendedorId);
            return View(produto);
        }




        // GET: Produtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", produto.Imagem.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }









    }
}
