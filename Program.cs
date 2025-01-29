using System.Text;
using ClearMind.ClearMind.Api.Controllers;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Application.Services.AnotacoesService;
using ClearMind.ClearMind.Application.Services.EmocaoService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var JWT_PASS = Environment.GetEnvironmentVariable("JWT_PASS") ?? "d5667v7byn89bv89vb7v7bnv823lky2fdfrfd325";
var JWT_AUDI = Environment.GetEnvironmentVariable("JWT_AUDI") ?? "ClearMind";
var JWT_ISSU = Environment.GetEnvironmentVariable("JWT_ISSU") ?? "ClearMind";

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
//configuracao do Authorize para colocar o token
builder.Services.AddSwaggerGen(
    c => { 
        c.SwaggerDoc("v1", new() { Title = "ClearMind API", Version = "v1" }); 
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Scheme = "bearer",
                In = ParameterLocation.Header
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new String[] {}
                }
            });
    }
); // Registrar Swagger

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT_PASS)),
        ValidateIssuer = true,
        ValidateAudience = true,
         ValidAudience = JWT_AUDI,
        ValidIssuer = JWT_ISSU,
        ClockSkew = TimeSpan.Zero
    };  
});

//Adicionando Escopo dos Serviços criados como Injeção de Dependencias
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<SetEmocaoService>();
builder.Services.AddScoped<getEmocoesPessoaService>();
builder.Services.AddScoped<countEmocoesPessoa>();
builder.Services.AddScoped<GenerateTokenPessoa>();
builder.Services.AddScoped<AnotacoesService>();
builder.Services.AddScoped<DeleteAnotacaoService>();


// Configuração do HttpClient e do GeminiClientService
builder.Services.AddHttpClient<GeminiClientService>(client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"); // Substitua pela URL correta
}).AddTypedClient((httpClient, serviceProvider) =>
{
    var emocaoService = serviceProvider.GetRequiredService<SetEmocaoService>();
    return new GeminiClientService(httpClient, apiKey, emocaoService);
});

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
