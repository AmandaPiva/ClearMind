using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados ao contêiner de serviços
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necessário para integrar Swagger
builder.Services.AddSwaggerGen(); // Registrar Swagger

//Adicionando Escopo dos Serviços criados como Injeção de Dependencias
builder.Services.AddScoped<PessoaService>();

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
