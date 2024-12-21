using System;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Data.Enuns;
using ClearMind.ClearMind.Data.Models;
using ClearMind.ClearMind.Data.Models.ConversaEstado;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiClientController : ControllerBase
    {
        private readonly SetEmocaoService _emocaoService;
        private readonly GeminiClientService _geminiClientService;

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
                // Obtém o estado atual da conversa
                var estadoConversa = await _emocaoService.ObterEstadoConversa(request.PessoaId);
                string contextoAtual = estadoConversa?.ContextoAtual ?? "Nenhuma emoção foi registrada anteriormente.";

                // Atualiza o contexto com a nova emoção
                string novoContexto = $"{contextoAtual} O usuário descreveu sua emoção como: {request.NomeEmocao}.";

                // Envia o contexto atualizado para a IA Gemini
                var respostaGemini = await _geminiClientService.SendPromptGeminiAsync(novoContexto);

                // Salva o novo estado da conversa no banco de dados
                await _emocaoService.SalvarConversaEstado(new ConversaEstado
                {
                    PessoaId = request.PessoaId,
                    ContextoAtual = novoContexto,
                    Finalizado = !respostaGemini.AguardaMaisInformacoes
                });

                // Salva a emoção associada à conversa
                var emocaoSalva = await _emocaoService.SetEmocao(new Emocao
                {
                    PessoaId = request.PessoaId,
                    NomeEmocao = request.NomeEmocao,
                    decisao = request.decisao
                });

                // Retorna a resposta da IA e o estado da conversa
                if (respostaGemini.AguardaMaisInformacoes)
                {
                    return Ok(new
                    {
                        Resposta = respostaGemini.Resposta,
                        EmocaoSalva = emocaoSalva,
                        Mensagem = "A IA está aguardando mais informações."
                    });
                }

                return Ok(new
                {
                    Resposta = respostaGemini.Resposta,
                    EmocaoSalva = emocaoSalva,
                    Mensagem = "Conversa finalizada com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }
}
