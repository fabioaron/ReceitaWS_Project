using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReceitaWS_Project.DTOs
{ 
public class LoginMongo
{
    public string Email { get; set; } = string.Empty; // Email do usuário (requerido)
    public string SenhaHash { get; set; } = string.Empty; // Hash da senha (requerido)
}
}