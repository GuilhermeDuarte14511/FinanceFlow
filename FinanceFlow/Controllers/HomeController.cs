using System.Diagnostics;
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
using FinanceFlow.Models;
using System.Security.Claims;

namespace FinanceFlow.Controllers
{
    [Authorize] // Garante que o usuário esteja autenticado
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransacaoService _transacaoService;
        private readonly ICategoriaService _categoriaService;

        public HomeController(ILogger<HomeController> logger, ITransacaoService transacaoService, ICategoriaService categoriaService)
        {
            _logger = logger;
            _transacaoService = transacaoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index(int mes = 0, int ano = 0)
        {
            if (mes == 0) mes = DateTime.Now.Month;
            if (ano == 0) ano = DateTime.Now.Year;

            int usuarioId;
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out usuarioId) || usuarioId == 0)
            {
                return RedirectToAction("Logout", "Login");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObterTransacoes(int mes, int? ano)
        {
            int usuarioId;
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out usuarioId) || usuarioId == 0)
            {
                return Unauthorized();
            }

            ano ??= DateTime.Now.Year;

            var totalReceitas = await _transacaoService.CalcularTotalReceitas(usuarioId, mes, ano.Value);
            var totalDespesas = await _transacaoService.CalcularTotalDespesas(usuarioId, mes, ano.Value);
            var transacoes = await _transacaoService.ObterTransacoesPorMes(usuarioId, mes, ano.Value);

            var listaTransacoes = transacoes
                .Select(t => new
                {
                    Id = t.Id,
                    Valor = t.Valor,
                    Descricao = t.Descricao,
                    Data = t.Data.ToString("yyyy-MM-dd"),
                    Tipo = t.Tipo.ToString(),
                    FormaPagamento = t.FormaPagamento.ToString()
                })
                .ToList();

            var response = new
            {
                totalReceitas = totalReceitas > 0 ? totalReceitas : 0M,
                totalDespesas = totalDespesas > 0 ? totalDespesas : 0M,
                transacoes = listaTransacoes
            };

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> ObterCategorias()
        {
            int usuarioId;
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out usuarioId) || usuarioId == 0)
            {
                return Unauthorized();
            }

            var categorias = await _categoriaService.ObterCategoriasPorUsuario(usuarioId);

            return Ok(categorias.Select(c => new { id = c.Id, nome = c.Nome }));
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarTransacao([FromBody] AdicionarTransacaoDto transacaoDto)
        {
            if (transacaoDto == null)
                return BadRequest("Dados inválidos!");

            int usuarioId;
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out usuarioId) || usuarioId == 0)
            {
                return Unauthorized();
            }

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
