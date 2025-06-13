using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
using DotNetEnv;
using ProdutosAPi.Models;
using ProdutosAPi.Data;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args); // Adicionar serviços ao container. (Registrador de serviços)

//Obtendo string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrando ApplicationDbContext com o provedor Npgsql (COnfigurando para usar o postgres)
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole> (options => {
  
    //Configuracoes de senha (exemplo)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
  
    //Configuracoes de Lockout (bloqueio)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    //Configuracoes de usuário
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>() //Diz ao Identity para usar o EF e seu DbContext
  .AddDefaultTokenProviders(); //Adiciona provedores para coisas como reset de senha e confiracoes de e-mail

  //Configuracao da autenticacao JWT Bearer
  //As chaves JWT (Issuer, AUdience, Key) geralmente vem do appsettings.json
  var jwtIssuer = builder.Configuration["JWT_ISSUER"];
  var jwtAudience = builder.Configuration["JWT_AUDIENCE"];
  var jwtKey = builder.Configuration["JWT_KEY"]!;

  builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}) .AddJwtBearer(options => {
  options.SaveToken = true; //Salvar o token no httpContext após validacao
  options.RequireHttpsMetadata = false; //Em produção, defina true
  options.TokenValidationParameters = new TokenValidationParameters() {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true, //Verifica se o token nao expirou
    ValidateIssuerSigningKey = true, //Valida a assinatura do token

    ValidAudience = jwtAudience, //Minha audiencia valida (ex:"seusite.com")
    ValidIssuer = jwtIssuer, // Meu emissor valido (ex:"seuapi.com")
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) //Minha chave secreta
  };
});

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

using (var scope = app.Services.CreateScope()) 
{
  var services = scope.ServiceProvider;
  try 
  {
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Initialize(userManager, roleManager); //Chama o método de iniciar a Seed
  }
  catch (Exception ex) 
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Um erro ocorreu durante o seeding do banco de dados do Identity.");
  }
}


app.UseHttpsRedirection();

app.UseAuthentication(); //Identifica quem é o usuario lendo o token JWT
app.UseAuthorization(); //Verifica o que o usuario autenticado pode fazer

app.MapControllers();

app.Run();
