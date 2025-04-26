using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReceitaWS_Project.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateToken(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("O e-mail não pode estar vazio.", nameof(email));
            }

            // Define as declarações do token (claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, email), // Identificador do assunto
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único do token
            };

            // Cria a chave de segurança usando a chave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            // Define as credenciais de assinatura com o algoritmo HMAC SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configura o token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // Configuração opcional de emissor
                audience: _jwtSettings.Audience, // Configuração opcional de público
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Expiração do token
                signingCredentials: creds
            );

            // Retorna o token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Classe de configuração do JWT
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty; // Chave secreta
        public string? Issuer { get; set; } // Opcional: emissor do token
        public string? Audience { get; set; } // Opcional: público-alvo do token
    }
}