using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Classes_Compartilhadas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtualMvc.Controllers
{

    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoriaService _categoriaService;

        public CategoriasController(ApplicationDbContext context, CategoriaService categoriaService)
        {
            _context = context;
            _categoriaService = categoriaService;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _context.Categoria.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();


            var categoria = await _categoriaService.ObterCategoriaPorId(id);

            if (categoria == null)        
                return NotFound();
            

            return View(categoria);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaService.InserirCategoria(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            

            var categoria = await _categoriaService.ObterCategoriaPorId(id);

            if (categoria == null)
                return NotFound();
            
            return View(categoria);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] Categoria categoria)
        {
            if (id != categoria.Id)
                return NotFound();
            

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaService.AtualizaCategoria(categoria);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }


        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();


            var categoria = await _categoriaService.ObterCategoriaPorId(id);

            if (categoria == null)
                return NotFound();
            

            return View(categoria);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _categoriaService.ObterCategoriaPorId(id);
            if (categoria == null)
                return NotFound();

            if (!await _categoriaService.CategoriaPodeSerExcluida(id))
            {
                ModelState.AddModelError(string.Empty, "Essa categoria Esta em uso");
                return View("Delete", categoria);
            }

            await _categoriaService.RemoverCategoria(categoria);
            return RedirectToAction(nameof(Index));


            
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }

    }
}

