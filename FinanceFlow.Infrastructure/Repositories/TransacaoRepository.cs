using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Interfaces;
using FinanceFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly FinanceFlowDbContext _context;

        public TransacaoRepository(FinanceFlowDbContext context)
        {
            _context = context;
        }

        public async Task<Transacao> GetByIdAsync(int id)
        {
            return await _context.Transacoes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transacao> GetByIdAndUsuarioIdAsync(int id, int usuarioId)
        {
            return await _context.Transacoes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Transacao>> GetByUsuarioIdAndMesAsync(int usuarioId, int mes, int ano)
        {
            return await _context.Transacoes
                .AsNoTracking()
                .Where(t => t.UsuarioId == usuarioId && t.Data.Month == mes && t.Data.Year == ano)
                .ToListAsync();
        }

        public async Task AddAsync(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transacao transacao)
        {
            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transacao transacao)
        {
            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();
        }
    }
}