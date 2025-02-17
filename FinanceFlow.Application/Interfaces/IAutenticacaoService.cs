using FinanceFlow.Domain.Entities;
using System.Security.Claims;

namespace FinanceFlow.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        ClaimsPrincipal AutenticarUsuario(Usuario usuario);
        string HashPassword(string senha);
        bool VerifyPassword(string senha, string senhaHash);
    }
}
