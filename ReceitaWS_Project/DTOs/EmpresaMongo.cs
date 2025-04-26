using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReceitaWS_Project.DTOs
{ 
public class EmpresaMongo
    {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Abertura { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string NaturezaJuridica { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
}
}