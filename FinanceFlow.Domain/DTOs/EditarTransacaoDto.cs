using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.DTOs
{
    public class EditarTransacaoDto
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public int Tipo { get; set; }
        public int CategoriaId { get; set; }
        public int FormaPagamento { get; set; }
    }
}
