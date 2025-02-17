using FinanceFlow.Application.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> AutenticarUsuarioAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || !VerificarSenha(senha, usuario.SenhaHash))
            {
                return null;
            }

            return usuario;
        }

        private bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var senhaBytes = Encoding.UTF8.GetBytes(senhaDigitada);
                var hashBytes = sha256.ComputeHash(senhaBytes);
                var hashString = Convert.ToBase64String(hashBytes);
                return hashString == senhaHash;
            }
        }
    }
}
