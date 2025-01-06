using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Data.Enuns;


namespace ClearMind.ClearMind.Data.Models
{
    [Table("Emocao")]
    public class Emocao
    {
        [Key]
        public int Id { get; set; }

        [StringLength(600)]
        public string NomeEmocao { get; set; } = string.Empty;

        [Required]
        public int PessoaId { get; set; }
         
        [ForeignKey(nameof(PessoaId))]
        public Pessoa? pessoa { get; set; }

        //Enum
        public Decisao decisao { get; set; } = Decisao.NONE; //Define como valor padrão NONE
    }
}