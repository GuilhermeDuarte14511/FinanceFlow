using FinanceFlow.Domain.Entities;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> AutenticarUsuarioAsync(string email, string senha);
    }
}
