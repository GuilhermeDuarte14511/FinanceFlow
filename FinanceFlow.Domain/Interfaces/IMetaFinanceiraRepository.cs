using FinanceFlow.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Interfaces
{
    public interface IMetaFinanceiraRepository
    {
        Task<MetaFinanceira> GetByIdAsync(int id);
        Task<IEnumerable<MetaFinanceira>> GetByUsuarioIdAsync(int usuarioId);
        Task AddAsync(MetaFinanceira meta);
        Task UpdateAsync(MetaFinanceira meta);
        Task DeleteAsync(int id);
    }
}
