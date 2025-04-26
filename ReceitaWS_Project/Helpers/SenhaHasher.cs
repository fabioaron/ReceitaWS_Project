using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

    namespace ReceitaWS_Project.Helpers
    {
        public static class SenhaHasher
        {
            public static string HashSenha(string senha)
            {
                using var sha256 = SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

