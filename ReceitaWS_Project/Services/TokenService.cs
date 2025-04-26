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
                throw new ArgumentException("O e-mail n�o pode estar vazio.", nameof(email));
            }

            // Define as declara��es do token (claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, email), // Identificador do assunto
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID �nico do token
            };

            // Cria a chave de seguran�a usando a chave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            // Define as credenciais de assinatura com o algoritmo HMAC SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configura o token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // Configura��o opcional de emissor
                audience: _jwtSettings.Audience, // Configura��o opcional de p�blico
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Expira��o do token
                signingCredentials: creds
            );

            // Retorna o token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Classe de configura��o do JWT
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty; // Chave secreta
        public string? Issuer { get; set; } // Opcional: emissor do token
        public string? Audience { get; set; } // Opcional: p�blico-alvo do token
    }
}