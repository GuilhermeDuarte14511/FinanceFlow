using System.Threading.Tasks;
using FinanceFlow.Domain.Entities;
using System.Collections.Generic;
using FinanceFlow.Domain.DTOs;

namespace FinanceFlow.Domain.Interfaces.Services
{
    public interface ITransacaoService
    {
        Task<IEnumerable<Transacao>> ObterTransacoesPorMes(int usuarioId, int mes, int ano);
        Task<decimal> CalcularTotalReceitas(int usuarioId, int mes, int ano);
        Task<decimal> CalcularTotalDespesas(int usuarioId, int mes, int ano);
        Task AdicionarTransacaoAsync(Transacao transacao);

        Task<Transacao> ObterTransacaoPorIdAsync(int transacaoId, int usuarioId);
        Task<bool> EditarTransacaoAsync(EditarTransacaoDto transacaoDto, int usuarioId);
        Task<bool> ExcluirTransacaoAsync(int transacaoId, int usuarioId);
    }
}
