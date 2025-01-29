using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Data.Models
{
    [Table("Anotacoes")]
    public class Anotacoes
    {
        [Key]
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(600)]
        public string Anotacao { get; set; } = string.Empty;
        //FK 
        [Required]
        public int IdEmocao { get; set; }

        [ForeignKey(nameof(IdEmocao))]
        [JsonIgnore] 
        public Emocao ?emocao { get; set; }
    }
}