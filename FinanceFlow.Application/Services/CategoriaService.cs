using FinanceFlow.Application.Interfaces;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Categoria>> ObterCategoriasPorUsuario(int usuarioId)
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return categorias.Where(c => c.UsuarioId == usuarioId || c.Padrao).ToList();
        }

        public async Task<Categoria> ObterPorId(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task Adicionar(Categoria categoria)
        {
            await _categoriaRepository.AddAsync(categoria);
        }

        public async Task Atualizar(Categoria categoria)
        {
            await _categoriaRepository.UpdateAsync(categoria);
        }

        public async Task Remover(int id)
        {
            await _categoriaRepository.DeleteAsync(id);
        }
    }
}
