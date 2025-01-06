using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services.AnotacoesService;
using ClearMind.ClearMind.Data.Models.Anotacoes;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnotacoesController : ControllerBase
    {
        private readonly AnotacoesService _anotacoesService;

        public AnotacoesController(AnotacoesService anotacoesService)
        {
            _anotacoesService = anotacoesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnotacao([FromBody] Anotacoes anotacao)
        {
            if (anotacao == null)
            {
                return BadRequest("Dados inválidos");
            }

            var anotacaoCriada = await _anotacoesService.CreateAnotacao(anotacao);

            return CreatedAtAction(
                nameof(CreateAnotacao),
                new { id = anotacaoCriada.Id },
                anotacaoCriada
            );
        }
    }
}