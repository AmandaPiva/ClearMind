using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Data.Models.Anotacoes
{
    [Table("Anotacoes")]
    public class Anotacoes
    {
        [Key]
        public int Id { get; set; }
        //FK 
        [Required]
        public int IdEmocao { get; set; }
        public Emocao emocao { get; set; }
    }
}