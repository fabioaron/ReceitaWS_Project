using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReceitaWS_Project.DTOs;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<UserMongo> _userCollection;

    public UserController()
    {
        // Conexão com o MongoDB
        const string connectionUri = "mongodb+srv://euaronsanto:9TLxZNwwkQt0DZlV@receitaws.rdptk71.mongodb.net/?retryWrites=true&w=majority&appName=ReceitaWS";
        var client = new MongoClient(connectionUri);
        var database = client.GetDatabase("ReceitaWS");
        _userCollection = database.GetCollection<UserMongo>("Users");
    }

    // Método para adicionar um novo usuário
    [HttpPost("add")]
    public IActionResult AddUser([FromBody] UserMongo newUser)
    {
        // Validação dos dados recebidos
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Nome) || string.IsNullOrWhiteSpace(newUser.SenhaHash))
        {
            return BadRequest(new { Message = "Dados inválidos. Certifique-se de fornecer Nome, Email e Senha." });
        }

        // Verificar se o usuário já existe na coleção
        var existingUser = _userCollection.Find(u => u.Email == newUser.Email).FirstOrDefault();
        if (existingUser != null)
        {
            return Conflict(new { Message = "O email já está em uso." });
        }

        // Hash da senha antes de salvar
        newUser.SenhaHash = BCrypt.Net.BCrypt.HashPassword(newUser.SenhaHash);

        try
        {
            // Inserir o novo usuário
            _userCollection.InsertOne(newUser);
            return Ok(new { Message = "Usuário registrado com sucesso!", User = newUser });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Erro interno ao registrar usuário: {ex.Message}" });
        }
    }
}