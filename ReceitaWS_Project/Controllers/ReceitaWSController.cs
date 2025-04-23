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
    [ProducesResponseType(typeof(EmpresaDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
    [ProducesResponseType((int)HttpStatusCode.GatewayTimeout)]
    public async Task<IActionResult> GetEmpresa(string cnpj)
    {
        // Validação básica para garantir que o CNPJ tenha 14 dígitos numéricos
        if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14 || !Regex.IsMatch(cnpj, @"^\d+$"))
        {
            return BadRequest("CNPJ inválido. Use 14 dígitos numéricos.");
        }

        // URL construída com base no CNPJ informado
        string url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return StatusCode((int)HttpStatusCode.TooManyRequests, "Limite de consultas excedido. Tente novamente mais tarde.");
            }

            if (response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                return StatusCode((int)HttpStatusCode.GatewayTimeout, "Timeout na consulta. Tente novamente mais tarde.");
            }

            if (response.IsSuccessStatusCode)
            {
                // Captura o JSON retornado pela API
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Configura as opções do deserializador (tornando-o insensível a maiúsculas/minúsculas)
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Desserializa o JSON para a classe EmpresaDto
                var empresa = JsonSerializer.Deserialize<EmpresaDto>(jsonResponse, options);

                // Retorna o objeto com os dados
                return Ok(empresa);
            }

            return StatusCode((int)response.StatusCode, $"Erro: {response.ReasonPhrase}");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Erro ao acessar a API: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}