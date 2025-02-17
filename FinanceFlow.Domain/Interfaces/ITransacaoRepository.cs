using FinanceFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<Transacao> GetByIdAsync(int id);
        Task<IEnumerable<Transacao>> GetByUsuarioIdAndMesAsync(int usuarioId, int mes, int ano);
        Task AddAsync(Transacao transacao);
        Task UpdateAsync(Transacao transacao);
        Task DeleteAsync(int id);
    }
}
