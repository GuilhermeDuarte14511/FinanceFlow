using FinanceFlow.Application.Interfaces;
using FinanceFlow.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinanceFlow.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        public ClaimsPrincipal AutenticarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                return null;
            }

            // Criando os claims (dados do usuário)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, "Usuario") // Adicionando Role padrão para usuários
            };

            // Configurando a identidade e principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public string HashPassword(string senha)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToHexString(hashBytes); // Converte para hexadecimal
        }

        public bool VerifyPassword(string senha, string senhaHash)
        {
            var hashedPassword = HashPassword(senha);
            return hashedPassword == senhaHash;
        }
    }
}
