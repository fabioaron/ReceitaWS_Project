using MongoDB.Driver;
using ReceitaWS_Project.DTOs;
using System;

public class EmpresaService
{
	private readonly IMongoCollection<EmpresaMongo> _empresaCollection;

	public EmpresaService(IMongoDatabase database)
	{
		// Inicializa a coleção de empresas
		_empresaCollection = database.GetCollection<EmpresaMongo>("Empresas");
	}

	public EmpresaMongo RegistrarEmpresa(string nome, string nomeFantasia, string cnpj, string situacao, string abertura, string tipo, string naturezaJuridica, string logradouro, string numero, string complemento, string bairro, string municipio, string uf, string cep)
	{
		// Cria um novo objeto de empresa
		var empresa = new EmpresaMongo
		{
			Nome = nome,
			NomeFantasia = nomeFantasia,
			Cnpj = cnpj,
			Situacao = situacao,
			Abertura = abertura,
			Tipo = tipo,
			NaturezaJuridica = naturezaJuridica,
			Logradouro = logradouro,
			Numero = numero,
			Complemento = complemento,
			Bairro = bairro,
			Municipio = municipio,
			Uf = uf,
			CEP = cep,
			DataRegistro = DateTime.UtcNow
		};

		try
		{
			// Verifica se a empresa já existe
			var existingEmpresa = _empresaCollection.Find(e => e.Cnpj == cnpj).FirstOrDefault();
			if (existingEmpresa != null)
			{
				throw new Exception("O CNPJ já está registrado.");
			}

			// Insere a nova empresa na coleção
			_empresaCollection.InsertOne(empresa);
			return empresa;
		}
		catch (Exception ex)
		{
			throw new Exception($"Erro ao registrar empresa: {ex.Message}");
		}
	}

	public EmpresaMongo ProcurarEmpresa(string cnpj)
	{
		try
		{
			// Busca a empresa pelo CNPJ
			return _empresaCollection.Find(e => e.Cnpj == cnpj).FirstOrDefault();
		}
		catch (Exception ex)
		{
			throw new Exception($"Erro ao buscar empresa: {ex.Message}");
		}
	}

	public List<EmpresaMongo> ListarEmpresas()
	{
		try
		{
			// Retorna todas as empresas da coleção
			return _empresaCollection.Find(FilterDefinition<EmpresaMongo>.Empty).ToList();
		}
		catch (Exception ex)
		{
			throw new Exception($"Erro ao listar empresas: {ex.Message}");
		}
	}
}