using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Data.Enuns;
using ClearMind.ClearMind.Data.Models;
using ClearMind.ClearMind.Data.Models.GeminiRequests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiClientController : ControllerBase
    {
        private readonly EmocaoService _emocaoService;
        private readonly GeminiClientService _geminiClientService;

         // Construtor com injeção de dependência
        public GeminiClientController(EmocaoService emocaoService, GeminiClientService geminiClientService)
        {
            _emocaoService = emocaoService ?? throw new ArgumentNullException(nameof(emocaoService));
            _geminiClientService = geminiClientService ?? throw new ArgumentNullException(nameof(geminiClientService));
        }

        [HttpPost("chat")]
        public async Task<IActionResult> EnviarMensagemEmocao([FromBody] Emocao request)
        {
          if (request == null)
          {
            return BadRequest(new { Erro = "O corpo da requisição não pode ser nulo." });
          }

          if(request.PessoaId <= 0)
          {
             return BadRequest(new { Erro = "O ID da pessoa é inválido." });
          }

          try
          {
          
            string contextoEmocional;

            if(request.decisao != Decisao.NONE)
            {
                 // Usa a emoção do enum `Decisao` se disponível
                contextoEmocional = $"O usuário está se sentindo {request.decisao}.";
            }
            else
            {
                contextoEmocional = $"O usuário descreveu sua emoção como: {request.NomeEmocao}.";
            }

            //Enviar prompt para a IA
            var resposta = await _geminiClientService.SendPromptGeminiAsync( contextoEmocional);

            //Salvando a emoção no banco de dados
            var saveEmotion = await _emocaoService.SetEmocao(request);
            return StatusCode(200, new { Resposta = resposta, Save = saveEmotion });
          }
          catch(Exception ex)
          {
            return StatusCode(500, new { Erro = ex.Message });
          }
        }
    }
}