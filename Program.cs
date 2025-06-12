using Microsoft.EntityFrameworkCore;

using ProdutosAPi.Data;
var builder = WebApplication.CreateBuilder(args); // Adicionar serviços ao container.

//Obtendo string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrando ApplicationDbContext com o provedor Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necessário para explorar os endpoints da API
builder.Services.AddSwaggerGen();// Adiciona o gerador do Swagger

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configura o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
