using Azure.Messaging;
using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Classes_Compartilhadas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace LojaVirtualApi.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoriaService _categoriaService;


        public CategoriasController(ApplicationDbContext context, CategoriaService categoriaService)
        {
            _context = context;
            _categoriaService = categoriaService;
        }




        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            if (!await _categoriaService.TemCategoria())
                return Problem("Erro ao criar uma Categoria, contate o suporte!");


            var categorias = await _categoriaService.TodasCategorias();
            return Ok(categorias);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            if (!await _categoriaService.TemCategoria())                
                return NotFound();
            

            var categoria = await _categoriaService.ObterCategoriaPorId(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            if (!await _categoriaService.TemCategoria())
                return Problem("Erro ao criar uma Categoria, contate o suporte!");


            if (!ModelState.IsValid)
            {                
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }
            await _categoriaService.InserirCategoria(categoria);


            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (!await _categoriaService.CategoriaExiste(id))
                return NotFound();

            if (!await _categoriaService.CategoriaPodeSerExcluida(id))
                return BadRequest("Não é possível excluir a categoria, ela está sendo usada.");

            var categoria = await _categoriaService.ObterCategoriaPorId(id);

            await _categoriaService.RemoverCategoria(categoria);

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return (_context.Categoria?.Any(c => c.Id == id)).GetValueOrDefault();
        }
    }
}
