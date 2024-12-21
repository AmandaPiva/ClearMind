using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClearMind.ClearMind.Application.Services
{
    public class GeminiClientService
    {
        //Propriedades de requisições http e armazenamento da chave da API do GEMINI
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GeminiClientService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

          // Classe para representar a resposta processada da IA
        public class GeminiResponse
        {
            public string Resposta { get; set; }
            public bool AguardaMaisInformacoes { get; set; }
        }

        //Prompt IA
        public async Task<GeminiResponse> SendPromptGeminiAsync( string? contextoEmocional)
        {
         
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

               // Processa a resposta JSON da API
            var responseContent = await response.Content.ReadAsStringAsync();

            // Tente interpretar a resposta da IA
            dynamic responseJson = JsonConvert.DeserializeObject(responseContent);
            string respostaTexto = responseJson?.generatedText ?? "A resposta da IA não pôde ser interpretada.";

            // Identificar se a IA está aguardando mais informações
            bool aguardaMaisInformacoes = respostaTexto.Contains("Deseja fornecer mais informações?", StringComparison.OrdinalIgnoreCase);

            // Retorna o objeto de resposta processada
            return new GeminiResponse
            {
                Resposta = respostaTexto,
                AguardaMaisInformacoes = aguardaMaisInformacoes
            };
                      
        }
    }
}