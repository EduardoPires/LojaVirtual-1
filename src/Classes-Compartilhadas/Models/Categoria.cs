using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Classes_Compartilhadas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres")]
        public string? Nome { get; set; }


        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {2} e {1} caracteres")]
        public string? Descricao { get; set; }



        [JsonIgnore]
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();



    }
}
