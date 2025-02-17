using System;
using FinanceFlow.Domain.Enums;

namespace FinanceFlow.Application.DTOs
{
    public class AdicionarTransacaoDto
    {
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public int CategoriaId { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
    }
}
