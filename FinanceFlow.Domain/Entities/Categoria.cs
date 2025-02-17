using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceFlow.Domain.Entities
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; } // Agora pode ser nulo para categorias globais

        [Required]
        [StringLength(100)] // Definir um tamanho máximo para evitar problemas no banco
        public string Nome { get; set; }

        public bool Padrao { get; set; } // Indica se é uma categoria global ou personalizada pelo usuário

        // Relacionamento com Usuário (se houver um usuário associado)
        public virtual Usuario Usuario { get; set; }

        // Relacionamento com Transações (uma categoria pode ter várias transações)
        public virtual ICollection<Transacao> Transacoes { get; set; }

        // Construtor para inicializar a lista de transações e evitar NullReferenceException
        public Categoria()
        {
            Transacoes = new HashSet<Transacao>();
        }
    }
}
