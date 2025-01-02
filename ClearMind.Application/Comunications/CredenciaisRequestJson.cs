using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Application.Comunications
{
    public class CredenciaisRequestJson
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}