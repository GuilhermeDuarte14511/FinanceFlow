using FinanceFlow.Domain.Enums;

namespace FinanceFlow.Domain.Entities
{
    public class Transacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public StatusPagamento Status { get; set; }
        public int CategoriaId { get; set; }
        public FormaPagamento FormaPagamento { get; set; }

        // Relacionamentos
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
    }
}
