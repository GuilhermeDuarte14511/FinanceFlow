using FinanceFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceFlow.Infrastructure.Data
{
    public class FinanceFlowDbContext : DbContext
    {
        public FinanceFlowDbContext(DbContextOptions<FinanceFlowDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MetaFinanceira> MetasFinanceiras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.Transacoes)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Categoria)
                .WithMany(c => c.Transacoes)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
