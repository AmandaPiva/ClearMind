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
            if (request == null)
            {
                return BadRequest(new { Erro = "O corpo da requisição não pode ser nulo." });
            }

            if (request.PessoaId <= 0)
            {
                return BadRequest(new { Erro = "O ID da pessoa é inválido." });
            }

            try
            {
                // Obtém o contexto emocional anterior do usuário
                string contextoEmocional = await ObterContextoEmocional(request.PessoaId);

                // Envia a mensagem ao Gemini com o contexto emocional
                string respostaGemini = await _geminiClientService.SendPromptGeminiAsync(contextoEmocional);

                // Salva a emoção associada à conversa
                var emocaoSalva = await _emocaoService.SetEmocao(new Emocao
                {
                    PessoaId = request.PessoaId,
                    NomeEmocao = request.NomeEmocao,
                    decisao = request.decisao
                });

                return Ok(new { Resposta = respostaGemini, EmocaoSalva = emocaoSalva });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        private async Task<string> ObterContextoEmocional(int pessoaId)
        {
            // Obtém a última emoção do usuário
            var emocao = await _emocaoService.ObterUltimaEmocaoAsync(pessoaId);

            if (emocao == null)
            {
                return "Nenhuma emoção foi registrada anteriormente.";
            }

            if (emocao.decisao != Decisao.NONE && emocao.NomeEmocao == "")
            {
                return $"O usuário está se sentindo {emocao.decisao}.";
            }
            else if (emocao.decisao == Decisao.NONE && emocao.NomeEmocao != "")
            {
                return $"O usuário descreveu sua emoção como: {emocao.NomeEmocao}.";
            }
            else if (emocao.decisao != Decisao.NONE && emocao.NomeEmocao != "" )
            {
                 return $"O usuário está se sentindo {emocao.decisao} e descreveu ela como: {emocao.NomeEmocao}.";
            }
           else
           {
             return $"Escolha uma emoção ou descreva ela com suas palavras";
           }
        }
    }
}
