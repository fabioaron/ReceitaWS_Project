using MongoDB.Driver;
using ReceitaWS_Project.DTOs;

public class UserService
{
	private readonly IMongoCollection<UserMongo> _userCollection;

	public UserService(IMongoDatabase database)
	{
		// Inicializa a coleção de usuários
		_userCollection = database.GetCollection<UserMongo>("Users");
	}

	public UserMongo RegistrarUser(string nome, string email, string senhaHash)
	{
		// Cria um novo objeto de usuário
		var user = new UserMongo
		{
			Nome = nome,
			Email = email,
			SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaHash), // Hash da senha
			DataCriacao = DateTime.UtcNow
		};

		try
		{
			// Verifica se o usuário já existe
			var existingUser = _userCollection.Find(u => u.Email == email).FirstOrDefault();
			if (existingUser != null)
			{
				throw new Exception("O email já está em uso.");
			}

			// Insere o novo usuário na coleção
			_userCollection.InsertOne(user);
			return user;
		}
		catch (Exception ex)
		{
			throw new Exception($"Erro ao registrar usuário: {ex.Message}");
		}
	}

	public UserMongo ProcurarUser(string email)
	{
		try
		{
			// Busca o usuário pelo email
			return _userCollection.Find(u => u.Email == email).FirstOrDefault();
		}
		catch (Exception ex)
		{
			throw new Exception($"Erro ao buscar usuário: {ex.Message}");
		}
	}
}