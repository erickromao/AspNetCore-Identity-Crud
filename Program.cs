using Microsoft.EntityFrameworkCore;

using ProdutosAPi.Data;
var builder = WebApplication.CreateBuilder(args); // Adicionar serviços ao container. (Registrador de serviços)

//Obtendo string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrando ApplicationDbContext com o provedor Npgsql (COnfigurando para usar o postgres)
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necessário para explorar os endpoints da API
builder.Services.AddSwaggerGen();// Adiciona o gerador do Swagger

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configura o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Middleware para gerar o JSON da especificacao
    app.UseSwaggerUI(); // Middleware para a intefarce gráfica do SWAGGER UI
    app.MapOpenApi(); // Mapeia os endpoints para servir o arquivo JSON da especificacao
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
