using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserMongo
{
    public ObjectId Id { get; set; } // ID gerado pelo MongoDB
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}