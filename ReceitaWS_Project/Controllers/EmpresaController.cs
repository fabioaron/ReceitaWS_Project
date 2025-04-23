using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReceitaWS_Project;

[ApiController]
[Route("api/[controller]")]
public class EmpresaController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public EmpresaController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("{cnpj}")]
    public async Task<IActionResult> ObterDados(string cnpj)
    {
        // Validação básica para garantir que o CNPJ tenha 14 dígitos numéricos
        if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14 || !Regex.IsMatch(cnpj, @"^\d+$"))
        {
            return BadRequest("CNPJ inválido. Use 14 dígitos numéricos.");
        }

        // URL construída com base no CNPJ informado
        var url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            // Lê o conteúdo da resposta (JSON) e desserializa para a classe Empresa, se essa classe corresponder
            var jsonContent = await response.Content.ReadAsStringAsync();

            // Caso possua a classe Empresa definida, desserialize-a:
            // Certifique-se de que os nomes dos atributos da classe Empresa correspondem aos do JSON recebido.
            var empresa = JsonSerializer.Deserialize<EmpresaDto>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return Ok(empresa);

            // Se preferir retornar a resposta como objeto dinâmico, poderia usar:
            // var result = JsonSerializer.Deserialize<dynamic>(jsonContent);
            // return Ok(result);
        }
        else
        {
            // Se a chamada externa falhar, retorna o respectivo status code e uma mensagem de erro
            return StatusCode((int)response.StatusCode, "Erro ao obter dados da API ReceitaWS.");
        }
    }
}
