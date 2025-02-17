using FinanceFlow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FinanceFlow.Application.DTOs
{
    public class DashboardResponse
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public List<TransacaoDto> Transacoes { get; set; }
    }

    public class TransacaoDto
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
    }
}
