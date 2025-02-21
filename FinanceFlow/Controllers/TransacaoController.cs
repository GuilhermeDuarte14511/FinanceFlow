using FinanceFlow.Application.Interfaces;
using FinanceFlow.Domain.Interfaces.Services;
using FinanceFlow.Application.DTOs;
using FinanceFlow.Domain.Entities;
using FinanceFlow.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FinanceFlow.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;
        private readonly ICategoriaService _categoriaService;

        public TransacaoController(ITransacaoService transacaoService, ICategoriaService categoriaService)
        {
            _transacaoService = transacaoService;
            _categoriaService = categoriaService;
        }

        [HttpGet("ObterTransacoes")]
        public async Task<IActionResult> ObterTransacoes(int mes, int? ano)
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0) return Unauthorized();

            ano ??= DateTime.Now.Year;

            var totalReceitas = await _transacaoService.CalcularTotalReceitas(usuarioId, mes, ano.Value);
            var totalDespesas = await _transacaoService.CalcularTotalDespesas(usuarioId, mes, ano.Value);
            var transacoes = await _transacaoService.ObterTransacoesPorMes(usuarioId, mes, ano.Value);

            var listaTransacoes = transacoes.Select(t => new
            {
                Id = t.Id,
                Valor = t.Valor,
                Descricao = t.Descricao,
                Data = t.Data.ToString("yyyy-MM-dd"),
                Tipo = t.Tipo.ToString(),
                FormaPagamento = t.FormaPagamento.ToString()
            }).ToList();

            return Ok(new
            {
                totalReceitas = totalReceitas > 0 ? totalReceitas : 0M,
                totalDespesas = totalDespesas > 0 ? totalDespesas : 0M,
                transacoes = listaTransacoes
            });
        }

        [HttpGet("ObterCategorias")]
        public async Task<IActionResult> ObterCategorias()
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0) return Unauthorized();

            var categorias = await _categoriaService.ObterCategoriasPorUsuario(usuarioId);
            return Ok(categorias.Select(c => new { id = c.Id, nome = c.Nome }));
        }

        [HttpPost("AdicionarTransacao")]
        public async Task<IActionResult> AdicionarTransacao([FromBody] AdicionarTransacaoDto transacaoDto)
        {
            if (transacaoDto == null)
                return BadRequest("Dados inválidos!");

            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0) return Unauthorized();

            var novaTransacao = new Transacao
            {
                UsuarioId = usuarioId,
                Valor = transacaoDto.Valor,
                Descricao = string.IsNullOrWhiteSpace(transacaoDto.Descricao) ? "Lançamento manual" : transacaoDto.Descricao,
                Data = transacaoDto.Data != DateTime.MinValue ? transacaoDto.Data : DateTime.UtcNow,
                Tipo = transacaoDto.Tipo,
                Status = StatusPagamento.Pago,
                CategoriaId = transacaoDto.CategoriaId > 0 ? transacaoDto.CategoriaId : 1,
                FormaPagamento = transacaoDto.FormaPagamento
            };

            await _transacaoService.AdicionarTransacaoAsync(novaTransacao);
            return Ok(new { sucesso = true, mensagem = "Transação adicionada com sucesso!" });
        }

        private int ObterUsuarioId()
        {
            return int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int usuarioId) ? usuarioId : 0;
        }
    }
}
