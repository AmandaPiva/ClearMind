using ClearMind.ClearMind.Api.Controllers;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados ao contêiner de serviços
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var apiKey = builder.Configuration["GeminiApiKey"];
if (string.IsNullOrEmpty(apiKey))
{
    throw new Exception("A chave da API do Gemini não foi configurada.");
}

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necessário para integrar Swagger
builder.Services.AddSwaggerGen(); // Registrar Swagger

//Adicionando Escopo dos Serviços criados como Injeção de Dependencias
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<SetEmocaoService>();


// Configuração do HttpClient e do GeminiClientService
builder.Services.AddHttpClient<GeminiClientService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"); // Substitua pela URL correta
}).AddTypedClient((httpClient, serviceProvider) =>
    new GeminiClientService(httpClient, apiKey));

var app = builder.Build();

//Configuração Swagger e Interface dele
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Redirecinando para a página do Swagger automaticamente
    app.MapGet("/", async context =>
    {
        context.Response.Redirect("/swagger");
    });
}

// Configura o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
