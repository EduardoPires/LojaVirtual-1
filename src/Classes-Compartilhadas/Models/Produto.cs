using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Classes_Compartilhadas.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres")]
        public string? Descricao { get; set; }

        
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        public string? Imagem { get; set; }


        [JsonIgnore]
        [NotMapped]
        public IFormFile? ImagemUpload { get; set; }

        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        public double Preco { get; set; }


        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [Range(0, 999999, ErrorMessage = "O campo {0} deve estar entre {1} e {2}")]
        public short Estoque { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        public DateTime DataAnuncio { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }


        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? VendedorId { get; set; }
        public Vendedor? Vendedor { get; set; }
    }
}
