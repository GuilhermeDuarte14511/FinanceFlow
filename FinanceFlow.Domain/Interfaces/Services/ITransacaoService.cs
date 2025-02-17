using System.Threading.Tasks;
using FinanceFlow.Domain.Entities;
using System.Collections.Generic;

namespace FinanceFlow.Domain.Interfaces.Services
{
    public interface ITransacaoService
    {
        Task<IEnumerable<Transacao>> ObterTransacoesPorMes(int usuarioId, int mes, int ano);
        Task<decimal> CalcularTotalReceitas(int usuarioId, int mes, int ano);
        Task<decimal> CalcularTotalDespesas(int usuarioId, int mes, int ano);
        Task AdicionarTransacaoAsync(Transacao transacao);
    }
}
