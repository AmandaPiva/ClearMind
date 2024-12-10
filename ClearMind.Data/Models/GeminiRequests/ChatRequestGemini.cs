using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Data.Models.GeminiRequests
{
    public class ChatRequestGemini
    {
         public int PessoaId { get; set; } // ID da pessoa que está enviando a mensagem
         public string Mensagem { get; set; } = string.Empty; // Mensagem enviada pelo usuário
    }
}