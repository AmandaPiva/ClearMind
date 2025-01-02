using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMind.ClearMind.Application.Comunications
{
    public class AuthenticationResponseJson
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;    
    }
}