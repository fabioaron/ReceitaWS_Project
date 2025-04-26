using ReceitaWS_Project.DTOs;
using MongoDB.Driver;

namespace ReceitaWS_Project.Services
{
    public class ValidateUser
    {
        private readonly UserService _userService;

        public ValidateUser(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Autentica um usuário verificando seu email e senha.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="senha">A senha fornecida para autenticação.</param>
        /// <returns>Verdadeiro se a autenticação for bem-sucedida; falso caso contrário.</returns>
        public bool AutenticarUser(string email, string senha)
        {
            // Busca o usuário no MongoDB usando o serviço de usuários
            var user = _userService.ProcurarUser(email);
            if (user == null)
                return false; // Usuário não encontrado

            // Verifica a senha fornecida comparando com o hash armazenado
            return VerificarSenha(senha, user.SenhaHash);
        }

        /// <summary>
        /// Verifica se a senha fornecida corresponde ao hash armazenado.
        /// </summary>
        /// <param name="senha">A senha fornecida.</param>
        /// <param name="hashArmazenado">O hash da senha armazenado no banco.</param>
        /// <returns>Verdadeiro se as senhas corresponderem; falso caso contrário.</returns>
        private bool VerificarSenha(string senha, string hashArmazenado)
        {
            // Compara a senha fornecida com o hash armazenado no MongoDB
            return BCrypt.Net.BCrypt.Verify(senha, hashArmazenado);
        }
    }
}