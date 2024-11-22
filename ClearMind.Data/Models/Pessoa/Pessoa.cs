using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Data.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(150)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [StringLength(150)]
        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}