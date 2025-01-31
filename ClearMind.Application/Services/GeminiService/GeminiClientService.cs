using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClearMind.ClearMind.Data.Enuns;
using ClearMind.ClearMind.Data.Models;
using ClearMind.ClearMind.Data.Models.PalavrasOfensivas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace ClearMind.ClearMind.Application.Services
{
    public class GeminiClientService
    {
        //Propriedades de requisições http e armazenamento da chave da API do GEMINI
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        private readonly SetEmocaoService _emocaoService;
        private readonly PalavrasOfensivasConf _palavrasOfensivasConfig;

        public GeminiClientService(HttpClient httpClient, string apiKey, SetEmocaoService emocaoService,  IOptions<PalavrasOfensivasConf> palavrasOfensivasOptions)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _emocaoService = emocaoService ?? throw new ArgumentNullException(nameof(emocaoService));
             _palavrasOfensivasConfig = palavrasOfensivasOptions.Value ?? throw new ArgumentNullException(nameof(palavrasOfensivasOptions));
        }

 // Método para censurar palavras ofensivas no texto
    private string CensurarTexto(string texto)
    {
        if (_palavrasOfensivasConfig.PalavrasOfensivas == null)
            return texto; // Se não houver palavras ofensivas cadastradas, retorna o texto original.

        foreach (var palavra in _palavrasOfensivasConfig.PalavrasOfensivas)
        {
            var regex = new Regex(@"\b" + Regex.Escape(palavra) + @"\b", RegexOptions.IgnoreCase);
            texto = regex.Replace(texto, "***");
        }
        return texto;
    }

    // Método para verificar se há palavras ofensivas e bloquear a mensagem
    private bool ContemPalavraProibida(string texto)
    {
        return _palavrasOfensivasConfig.PalavrasOfensivas?.Any(palavra =>
            Regex.IsMatch(texto, @"\b" + Regex.Escape(palavra) + @"\b", RegexOptions.IgnoreCase)) ?? false;
    }
      public async Task<(string RespostaGemini, Emocao EmocaoSalva)> ProcessarMensagemComEmocaoAsync(Emocao request)
        {
            if (request == null)
                throw new ArgumentNullException("Request nulo", nameof(request));

            // Obtém o contexto emocional (se existir)
            var contextoEmocional = await ObterContextoEmocional(request.PessoaId, request);


            if (contextoEmocional == null)
            {
                throw new Exception("Contexto emocianal não encontrado");
            }
            else
            {
                // Envia o prompt para a API do Gemini usando o contexto emocional existente
                string respostaGemini = await SendPromptGeminiAsync(contextoEmocional);

                return (respostaGemini, null); // Retorna null para EmocaoSalva, já que nenhuma nova emoção foi criada
            }
        }


        private async Task<string> ObterContextoEmocional(int pessoaId, Emocao request)
        {
              // Verifica se a emoção contém conteúdo ofensivo ANTES de salvar no banco de dados
            if (ContemConteudoOfensivo(request.NomeEmocao))
            {
                throw new Exception("Este tipo de mensagem é ofensiva e contra nossas diretrizes.");
            }
            //enviando a nova emoção para o banco de dados
            var novaEmocao = await _emocaoService.SetEmocao(new Emocao
                {
                    PessoaId = request.PessoaId,
                    NomeEmocao = request.NomeEmocao,
                    decisao = request.decisao
                });

            if(novaEmocao.decisao != Decisao.NONE && novaEmocao.NomeEmocao == "string")
            {
                return $"O usuário está se sentindo com {novaEmocao.decisao}";
            }
            else if (novaEmocao.decisao == Decisao.NONE && novaEmocao.NomeEmocao != "string")
            {
                return $"O usuário descreveu sua emoção como: {novaEmocao.NomeEmocao}";
            }
            else if(novaEmocao.decisao != Decisao.NONE && novaEmocao.NomeEmocao != "string")
            {
                return $"O usuário está se sentindo {novaEmocao.decisao} e descreveu ela como {novaEmocao.NomeEmocao}";
            }
            else 
            {
                return "O usuário não descreveu sua emoção";
            }
           
        }

        //Prompt IA
        public async Task<string> SendPromptGeminiAsync( string? contextoEmocional)
        {
         
          // Bloquear mensagens que contenham palavras ofensivas
            if (ContemPalavraProibida(contextoEmocional))
                return "Sua mensagem contém linguagem inapropriada.";

            // Censurar palavras ofensivas antes de enviar
            contextoEmocional = CensurarTexto(contextoEmocional);

            var fullPrompt = $"{contextoEmocional}";

            // Ajustando o formato conforme o exemplo do curl
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = fullPrompt
                            }
                        }
                    }
                }
            };
            // Criando a URL corretamente com a chave da API (Essa URL vem da doc do Gemini)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            //transformando em formato JSON
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na chamada da API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

           return await response.Content.ReadAsStringAsync();
           
        }

        public bool ContemConteudoOfensivo(string texto)
        {

            // Verifica se alguma palavra ofensiva está presente na lista do appsettings.json
            if (_palavrasOfensivasConfig?.PalavrasOfensivas != null)
            {
                if (_palavrasOfensivasConfig.PalavrasOfensivas.Any(palavra => texto.Contains(palavra, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}