ReceitaWS Project
Este é um projeto para gerenciar Usuários e Empresas, desenvolvido em C# utilizando ASP.NET Core e integrado com o MongoDB Atlas como banco de dados. A aplicação conta com funcionalidades de registro e autenticação de usuários, além de registro e consulta de empresas.

Estrutura do Projeto
Tecnologias Usadas
Backend:

ASP.NET Core 7.0

MongoDB Driver para C#

JWT Authentication

BCrypt para criptografia de senha

Banco de Dados:

MongoDB (via MongoDB Atlas)

Ferramentas e Testes:

Postman para testes de endpoints

Instalação
Clone o repositório:

bash
git clone <link-do-repositorio>
cd ReceitaWS_Project
Configuração do MongoDB:

Certifique-se de que o cluster no MongoDB Atlas está ativo.

Atualize a URI de conexão no arquivo Program.cs:

csharp
const string connectionUri = "sua-string-de-conexao";
Configuração do JWT:

Adicione o JwtSettings ao arquivo appsettings.json:

json
{
  "JwtSettings": {
    "Secret": "seu-segredo-unico",
    "Issuer": "ReceitaWS_Project",
    "Audience": "ReceitaWS_Project",
    "TokenExpiryInMinutes": 60
  }
}
Execute o projeto:

Compile e inicie o servidor:

bash
dotnet run
Testar Endpoints:

Use o Postman para validar os endpoints descritos abaixo.

Endpoints
Usuários
Registrar Usuário:

POST /api/users/register

Body (JSON):

json
{
  "nome": "Fabio",
  "email": "fabio@example.com",
  "senhaHash": "senha123"
}
Login de Usuário:

POST /api/users/login

Body (JSON):

json
{
  "email": "fabio@example.com",
  "senhaHash": "senha123"
}
Empresas
Registrar Empresa:

POST /api/empresas/register

Body (JSON):

json
{
  "nome": "Empresa Exemplo",
  "nomeFantasia": "Fantasia Exemplo",
  "cnpj": "12345678000199",
  "situacao": "Ativa",
  "abertura": "2023-01-01",
  "tipo": "Matriz",
  "naturezaJuridica": "Sociedade Limitada",
  "logradouro": "Rua Exemplo",
  "numero": "123",
  "complemento": "Sala 45",
  "bairro": "Centro",
  "municipio": "São Paulo",
  "uf": "SP",
  "cep": "01001001"
}
Listar Empresas:

GET /api/empresas

Buscar Empresa por CNPJ:

GET /api/empresas/{cnpj}

Problemas Conhecidos
O registro de usuários pode falhar com erro 500 se as configurações de MongoDB ou JWT não estiverem corretas.

O endpoint /api/users/login não retorna tokens JWT atualmente.

Validações adicionais precisam ser implementadas nos campos enviados pelos endpoints.

Melhorias Planejadas
Autenticação JWT: Adicionar geração e validação de tokens JWT para usuários.

Logs: Melhorar a captura de erros e logs para debug.

Testes Automatizados: Implementar testes unitários para garantir robustez.

Frontend: Criar interface usando React ou Angular.

Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou enviar pull requests.
