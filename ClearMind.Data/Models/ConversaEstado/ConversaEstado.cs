using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Data.Models.ConversaEstado
{
    public class ConversaEstado
    {
        [Key]
        public int PessoaId { get; set; }	
        
        [StringLength(150)]
        public string ContextoAtual { get; set; } = string.Empty;
        public bool Finalizado { get; set; } = false;
    }
}