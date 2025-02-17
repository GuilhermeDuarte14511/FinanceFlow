using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Interfaces;
using FinanceFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceFlow.Infrastructure.Repositories
{
    public class MetaFinanceiraRepository : IMetaFinanceiraRepository
    {
        private readonly FinanceFlowDbContext _context;

        public MetaFinanceiraRepository(FinanceFlowDbContext context)
        {
            _context = context;
        }

        public async Task<MetaFinanceira> GetByIdAsync(int id)
        {
            return await _context.MetasFinanceiras.FindAsync(id);
        }

        public async Task<IEnumerable<MetaFinanceira>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.MetasFinanceiras
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task AddAsync(MetaFinanceira meta)
        {
            _context.MetasFinanceiras.Add(meta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetaFinanceira meta)
        {
            _context.MetasFinanceiras.Update(meta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var meta = await _context.MetasFinanceiras.FindAsync(id);
            if (meta != null)
            {
                _context.MetasFinanceiras.Remove(meta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
