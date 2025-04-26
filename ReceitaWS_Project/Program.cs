using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceitaWS_Project.Services;
using ReceitaWS_Project.DTOs;
using MongoDB.Driver;
using BCrypt.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

internal class Program
{
    private static WebApplicationBuilder _builder;

    public static void Main(string[] args)
    {
        // Inicialização do ASP.NET Core
        _builder = WebApplication.CreateBuilder(args);

        // Configurar os serviços
        ConfigureServices();
        var app = _builder.Build();

        // Configurar o pipeline de middleware
        ConfigurePipeline(app);

        // Rodar o aplicativo
        app.Run();
    }

    private static void ConfigureServices()
    {
        // Configuração de conexão com MongoDB
        _builder.Services.AddSingleton<IMongoDatabase>(sp =>
        {
            const string connectionUri = "mongodb+srv://euaronsanto:9TLxZNwwkQt0DZlV@receitaws.rdptk71.mongodb.net/?retryWrites=true&w=majority&appName=ReceitaWS";
            var client = new MongoClient(connectionUri);
            return client.GetDatabase("ReceitaWS");
        });

        // Configuração de JwtSettings
        _builder.Services.Configure<JwtSettings>(_builder.Configuration.GetSection("JwtSettings"));
        _builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<JwtSettings>>().Value);

        // Registro de serviços
        _builder.Services.AddScoped<UserService>();
        _builder.Services.AddScoped<EmpresaService>();
        _builder.Services.AddScoped<ValidateUser>();
        _builder.Services.AddTransient<TokenService>();
        _builder.Services.AddControllers();
        _builder.Services.AddHttpClient();

        // Configurar CORS para permitir o React
        _builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000") // URL do frontend React
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Configuração de compressão de resposta
        _builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Middleware básico
        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseCors("AllowReactApp");

        // Servir arquivos estáticos (para React)
        app.UseStaticFiles();

        // Mapear controladores
        app.MapControllers();

        // Endpoint para verificar conexão com MongoDB
        app.MapGet("/mongo-check", async (IMongoDatabase database) =>
        {
            try
            {
                var result = await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
                return Results.Ok("MongoDB conectado com sucesso!");
            }
            catch (Exception ex)
            {
                return Results.Problem("Falha na conexão com MongoDB: " + ex.Message);
            }
        });

        // Endpoint para registro de usuários
        app.MapPost("/api/users/register", async (HttpContext http, UserService userService) =>
        {
            try
            {
                var userData = await http.Request.ReadFromJsonAsync<UserMongo>();

                if (userData == null || string.IsNullOrWhiteSpace(userData.Email) || string.IsNullOrWhiteSpace(userData.Nome) || string.IsNullOrWhiteSpace(userData.SenhaHash))
                {
                    Console.WriteLine("Dados inválidos recebidos.");
                    return Results.BadRequest(new { Message = "Dados inválidos. Certifique-se de fornecer Nome, Email e Senha." });
                }

                var registeredUser = userService.RegistrarUser(userData.Nome, userData.Email, userData.SenhaHash);
                return Results.Ok(new { Message = "Usuário registrado com sucesso!", User = registeredUser });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        });

        // Endpoint para autenticação de usuários
        app.MapPost("/api/users/login", async (HttpContext http, ValidateUser validateUser) =>
        {
            var loginData = await http.Request.ReadFromJsonAsync<UserMongo>();

            if (loginData == null || string.IsNullOrWhiteSpace(loginData.Email) || string.IsNullOrWhiteSpace(loginData.SenhaHash))
            {
                return Results.BadRequest(new { Message = "Dados inválidos. Certifique-se de fornecer email e senha." });
            }

            var isAuthenticated = validateUser.AutenticarUser(loginData.Email, loginData.SenhaHash);

            if (!isAuthenticated)
                return Results.Unauthorized();

            return Results.Ok(new { Message = "Login bem-sucedido!" });
        });

        // Endpoint para registro de empresas
        app.MapPost("/api/empresas/register", (EmpresaService empresaService, [FromBody] EmpresaMongo empresaDto) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empresaDto.Cnpj) || !Regex.IsMatch(empresaDto.Cnpj, @"^\d+$"))
                {
                    return Results.BadRequest(new { Message = "CNPJ inválido ou não preenchido!" });
                }

                var result = empresaService.RegistrarEmpresa(
                    empresaDto.Nome,
                    empresaDto.NomeFantasia,
                    empresaDto.Cnpj,
                    empresaDto.Situacao,
                    empresaDto.Abertura,
                    empresaDto.Tipo,
                    empresaDto.NaturezaJuridica,
                    empresaDto.Logradouro,
                    empresaDto.Numero,
                    empresaDto.Complemento,
                    empresaDto.Bairro,
                    empresaDto.Municipio,
                    empresaDto.Uf,
                    empresaDto.CEP
                );

                return Results.Ok(new { Message = "Empresa registrada com sucesso!", Empresa = result });
            }
            catch (Exception ex)
            {
                return Results.Conflict(new { Error = ex.Message });
            }
        });

        // Endpoint para listar empresas
        app.MapGet("/api/empresas", (EmpresaService empresaService) =>
        {
            try
            {
                var empresas = empresaService.ListarEmpresas();
                return Results.Ok(empresas);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao buscar empresa: {ex.Message}");
            }
        });
        // Endpoint para buscar empresa por CNPJ
        app.MapGet("/api/empresas/{cnpj}", (EmpresaService empresaService, string cnpj) =>
        {
            try
            {
                var empresa = empresaService.ProcurarEmpresa(cnpj);

                if (empresa == null)
                {
                    return Results.NotFound(new { Message = "Empresa não encontrada!" });
                }

                return Results.Ok(empresa);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao buscar empresa: {ex.Message}");
            }
        });
    }
}