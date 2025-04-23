using System.Text.Json.Serialization;

namespace ReceitaWS_Project 
{ 
public class EmpresaDto
{
    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; }

    [JsonPropertyName("uf")]
    public string Uf { get; set; }

    // Adicione outras propriedades conforme os dados retornados pela API.
}
}