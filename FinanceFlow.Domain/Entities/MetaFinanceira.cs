namespace FinanceFlow.Domain.Entities
{
    public class MetaFinanceira
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public decimal ValorMeta { get; set; }
        public decimal ValorAtual { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataFinal { get; set; }

        // Relacionamento
        public Usuario Usuario { get; set; }
    }
}
