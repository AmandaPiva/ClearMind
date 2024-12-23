using System;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Data.Enuns;
using ClearMind.ClearMind.Data.Models;
using ClearMind.ClearMind.Data.Models.GeminiRequests;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiClientController : ControllerBase
    {
        private readonly SetEmocaoService _emocaoService;
        private readonly GeminiClientService _geminiClientService;

        // Construtor para injeção de dependências
        public GeminiClientController(SetEmocaoService emocaoService, GeminiClientService geminiClientService)
        {
            _emocaoService = emocaoService ?? throw new ArgumentNullException(nameof(emocaoService));
            _geminiClientService = geminiClientService ?? throw new ArgumentNullException(nameof(geminiClientService));
        }

        [HttpPost("chat")]
        public async Task<IActionResult> EnviarMensagemEmocao([FromBody] Emocao request)
        {
            if (request == null || request.PessoaId <= 0)
            {
                return BadRequest(new { Erro = "Requisição inválida. Verifique os dados enviados." });
            }

            try
            {
               var resultado = await _geminiClientService.ProcessarMensagemComEmocaoAsync(request);

                return Ok(new { Resposta = resultado.RespostaGemini, EmocaoSalva = resultado.RespostaGemini });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

   
    }
}
