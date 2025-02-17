namespace FinanceFlow.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public ICollection<Transacao> Transacoes { get; set; }
        public ICollection<Categoria> CategoriasPersonalizadas { get; set; }
    }
}
