using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmocaoController : ControllerBase
    {
        //propriedade somente leitura que da acesso ao Service
        private readonly EmocaoService _emocaoService;

        public EmocaoController(EmocaoService emocaoService)
        {
            _emocaoService = emocaoService;
        }

        [HttpPost]
        public async Task<IActionResult> SetEmocao([FromBody] Emocao emocao)
        {
            if(emocao == null)
            {
                return BadRequest("Dados inválidos");
            }

            var emocaoCriada = await _emocaoService.SetEmocao(emocao);

            return CreatedAtAction(
                nameof(SetEmocao), 
                new { id = emocaoCriada.Id }, 
                emocaoCriada
            );
        }
    }
}