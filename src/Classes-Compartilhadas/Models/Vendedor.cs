using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Classes_Compartilhadas.Models
{
    public class Vendedor
    {

        [JsonIgnore]
        [ScaffoldColumn(false)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "o campo {0} precisa ter entre {1} e {2} caracteres")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [DataType(DataType.Date, ErrorMessage = "O campo {0} esta fora do formato correto")]
        [Display(Name = "Data de Nascimento")]//é o jeito que vai ser exibido nos formularios 
        public DateTime DataNascimento { get; set; }


        [JsonIgnore]
        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [StringLength(60, ErrorMessage = "o campo {0} precisa ter entre {1} e {2} caracteres")]
        [EmailAddress(ErrorMessage = "O campo {0} esta fora do formato valido")]
        [NotMapped]
        public string? Email { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [Display(Name = "Confirme o Email")]//é o jeito que vai ser exibido nos formularios 
        [Compare("Email", ErrorMessage = "Os emails informados sao diferentes ")]
        [NotMapped]
        public string? EmailConformacao { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "o campo{0} é obrigatorio")]
        [NotMapped]
        public string? Senha { get; set; }


    }
}
