using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Classes_Compartilhadas.Models
{
    public class ProdutoDTO
    {
        [Required]
        [StringLength(300, MinimumLength = 2)]
        public string? Descricao { get; set; }

        public IFormFile? ImagemUpload { get; set; }

        [Required]
        public double Preco { get; set; }

        [Required]
        [Range(0, 999999)]
        public short Estoque { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public string? VendedorId { get; set; }
    }
}
