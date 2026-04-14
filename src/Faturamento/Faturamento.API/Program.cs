using Faturamento.Application.Interfaces;
using Faturamento.Application.Services;
using Faturamento.Domain.Interfaces;
using Faturamento.Infrastructure.Clients;
using Faturamento.Infrastructure.Data;
using Faturamento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FaturamentoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();
builder.Services.AddScoped<INotaFiscalService, NotaFiscalService>();

builder.Services.AddHttpClient<IEstoqueClient, EstoqueClient>(client =>
    {
        //Lógica para rodar no docker e local
        client.BaseAddress = new Uri(builder.Configuration["EstoqueServiceUrl"] ?? "http://localhost:5027/");
    })
    //Política de retry para lidar com falhas transitórias na comunicação HTTP
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Faturamento API v1");
    });
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FaturamentoDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations no banco de dados.");
    }
}

app.Run();