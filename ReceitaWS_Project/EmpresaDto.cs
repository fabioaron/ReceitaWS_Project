using System.Text.Json.Serialization;

namespace ReceitaWS_Project 
{
    public class AtividadePrincipalDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
    public class EmpresaDto
{
  

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("fantasia")]
        public string NomeFantasia { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("abertura")]
        public string Abertura { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("natureza_juridica")]
        public string NaturezaJuridica { get; set; }

        [JsonPropertyName("atividade_principal")]
        public AtividadePrincipalDto[] AtividadePrincipal { get; set; }


        [JsonPropertyName("lougradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("municipio")]
        public string Municipio { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("cep")]
        public string CEP { get; set; }


    }
}