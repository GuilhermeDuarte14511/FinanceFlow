using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using FinanceFlow.Domain.Interfaces;
using FinanceFlow.Domain.Interfaces.Services;
using FinanceFlow.Domain.DTOs;

namespace FinanceFlow.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IEnumerable<Transacao>> ObterTransacoesPorMes(int usuarioId, int mes, int ano)
        {
            return await _transacaoRepository.GetByUsuarioIdAndMesAsync(usuarioId, mes, ano);
        }

        public async Task<decimal> CalcularTotalReceitas(int usuarioId, int mes, int ano)
        {
            var transacoes = await ObterTransacoesPorMes(usuarioId, mes, ano);
            return transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
        }

        public async Task<decimal> CalcularTotalDespesas(int usuarioId, int mes, int ano)
        {
            var transacoes = await ObterTransacoesPorMes(usuarioId, mes, ano);
            return transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
        }

        public async Task AdicionarTransacaoAsync(Transacao transacao)
        {
            await _transacaoRepository.AddAsync(transacao);
        }

        public async Task<Transacao> ObterTransacaoPorIdAsync(int transacaoId, int usuarioId)
        {
            return await _transacaoRepository.GetByIdAndUsuarioIdAsync(transacaoId, usuarioId);
        }

        public async Task<bool> EditarTransacaoAsync(EditarTransacaoDto transacaoDto, int usuarioId)
        {
            var transacao = await _transacaoRepository.GetByIdAndUsuarioIdAsync(transacaoDto.Id, usuarioId);

            if (transacao == null)
                return false;

            transacao.Valor = transacaoDto.Valor;
            transacao.Tipo = (TipoTransacao)transacaoDto.Tipo;
            transacao.CategoriaId = transacaoDto.CategoriaId;
            transacao.FormaPagamento = (FormaPagamento)transacaoDto.FormaPagamento;

            await _transacaoRepository.UpdateAsync(transacao);
            return true;
        }

        public async Task<bool> ExcluirTransacaoAsync(int transacaoId, int usuarioId)
        {
            var transacao = await _transacaoRepository.GetByIdAndUsuarioIdAsync(transacaoId, usuarioId);

            if (transacao == null)
                return false;

            await _transacaoRepository.DeleteAsync(transacao);
            return true;
        }
    }
}