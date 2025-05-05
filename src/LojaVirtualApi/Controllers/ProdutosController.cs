using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Classes_Compartilhadas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtualApi.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    [Authorize]
    public class ProdutosController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ProdutoService _produtoService;

        public ProdutosController(ApplicationDbContext context, ProdutoService produtoService)
        {
            _context = context;
            _produtoService = produtoService;
        }




        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            if (!await _produtoService.TemProduto())
                return Problem("Erro ao criar um produto, contate o suporte!");


            var produtos = await _produtoService.TodosProdutos();
            return Ok(produtos);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            if (!await _produtoService.TemProduto())
                return NotFound();


            var produto = await _produtoService.ObterProdutoPorId(id);

            if (produto == null)
                return NotFound();
           

            return produto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Produto>> PostProduto([FromForm] ProdutoDTO dto)
        {
            if (!await _produtoService.TemProduto())
                return Problem("Erro ao criar um Produto, contate o suporte!");

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            // Mapeamento manual do DTO para a entidade Produto
            var produto = new Produto
            {
                Descricao = dto.Descricao,
                ImagemUpload = dto.ImagemUpload,
                Preco = dto.Preco,
                Estoque = dto.Estoque,
                CategoriaId = dto.CategoriaId,
                VendedorId = dto.VendedorId,
                DataAnuncio = DateTime.Now
            };

            
            if (dto.ImagemUpload != null)
            {
                

                produto.Imagem = await _produtoService.InserirImagemAsync(dto.ImagemUpload);
            }


            await _produtoService.InserirProduto(produto);

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }



        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutProduto(int id, [FromForm] ProdutoDTO dto)
        {
            if (!await _produtoService.ProdutoPodeSerExcluido(id, dto.VendedorId))
                return BadRequest("Voce nao è proprietario deste anuncio!!");


            var produtoExistente = await _produtoService.ObterProdutoPorId(id);

            if (produtoExistente == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }


            produtoExistente.Descricao = dto.Descricao;
            produtoExistente.Preco = dto.Preco;
            produtoExistente.Estoque = dto.Estoque;
            produtoExistente.CategoriaId = dto.CategoriaId;
            produtoExistente.VendedorId = dto.VendedorId;

            if (dto.ImagemUpload != null)
            {
                produtoExistente.Imagem = await _produtoService.AtualizaImagemAsync(dto.ImagemUpload, produtoExistente.Imagem);
            }

            await _produtoService.AtualizarProduto(produtoExistente);

            return NoContent();
        }



        [HttpDelete("{id:int}/{id_vendeedor}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteProduto(int id, string id_vendeedor)
        {
            if (!await _produtoService.ProdutoExiste(id))
                return NotFound();

            if (!await _produtoService.ProdutoPodeSerExcluido(id, id_vendeedor))
                return BadRequest("Voce nao è proprietario deste anuncio!!");

            var produto = await _produtoService.ObterProdutoPorId(id);

            await _produtoService.RemoverProduto(produto);

            return NoContent();
        }








    }
}
