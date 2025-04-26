using Microsoft.AspNetCore.Mvc;
using ReceitaWS_Project.DTOs;
using ReceitaWS_Project.Services;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/empresas")]
public class EmpresaController : ControllerBase
{
	private readonly EmpresaService _empresaService;

	public EmpresaController(EmpresaService empresaService)
	{
		_empresaService = empresaService; // Inicializando o serviço
	}

	[HttpPost("register")]
	public IActionResult RegistrarEmpresa([FromBody] EmpresaMongo empresaDto)
	{
		// Validação básica do CNPJ
		if (string.IsNullOrWhiteSpace(empresaDto.Cnpj) || empresaDto.Cnpj.Length != 14 || !Regex.IsMatch(empresaDto.Cnpj, @"^\d+$"))
		{
			return BadRequest(new { Message = "CNPJ inválido. Certifique-se de fornecer 14 dígitos numéricos." });
		}

		try
		{
			// Registrar empresa usando o serviço
			var result = _empresaService.RegistrarEmpresa(
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

			if (result == null)
			{
				return Conflict(new { Message = "Empresa já registrada no banco de dados." });
			}

			return Ok(new { Message = "Empresa registrada com sucesso!", Empresa = result });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "Erro ao registrar empresa.", Detalhes = ex.Message });
		}
	}

	[HttpGet]
	public IActionResult GetEmpresas()
	{
		try
		{
			// Buscar todas as empresas usando o serviço
			var empresas = _empresaService.ListarEmpresas();
			return Ok(empresas);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "Erro ao buscar empresas.", Detalhes = ex.Message });
		}
	}

	[HttpGet("{cnpj}")]
	public IActionResult ProcurarEmpresa(string cnpj)
	{
		// Validação básica do CNPJ
		if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14 || !Regex.IsMatch(cnpj, @"^\d+$"))
		{
			return BadRequest(new { Message = "CNPJ inválido. Certifique-se de fornecer 14 dígitos numéricos." });
		}

		try
		{
			// Procurar empresa pelo CNPJ usando o serviço
			var empresa = _empresaService.ProcurarEmpresa(cnpj);
			if (empresa == null)
			{
				return NotFound(new { Message = "Empresa não encontrada." });
			}

			return Ok(empresa);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { Error = "Erro ao buscar empresa.", Detalhes = ex.Message });
		}
	}
}