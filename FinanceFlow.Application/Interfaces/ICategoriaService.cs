using FinanceFlow.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObterCategoriasPorUsuario(int usuarioId);
        Task<Categoria> ObterPorId(int id);
        Task Adicionar(Categoria categoria);
        Task Atualizar(Categoria categoria);
        Task Remover(int id);
    }
}
