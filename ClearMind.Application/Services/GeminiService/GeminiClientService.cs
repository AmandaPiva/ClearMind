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

        //Prompt IA
        public async Task<string> SendPromptGeminiAsync( string? contextoEmocional)
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
           
           return await response.Content.ReadAsStringAsync();
           
        }
    }
}