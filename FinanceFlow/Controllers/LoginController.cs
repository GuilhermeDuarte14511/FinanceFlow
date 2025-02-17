using FinanceFlow.Application.DTOs;
using FinanceFlow.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceFlow.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAutenticacaoService _autenticacaoService;

        public LoginController(IUsuarioService usuarioService, IAutenticacaoService autenticacaoService)
        {
            _usuarioService = usuarioService;
            _autenticacaoService = autenticacaoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Autenticar(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { sucesso = false, mensagem = "Preencha todos os campos corretamente." });
            }

            var usuario = await _usuarioService.AutenticarUsuarioAsync(model.Email, model.Senha);

            if (usuario == null)
            {
                return Json(new { sucesso = false, mensagem = "Login inválido! Verifique seus dados." });
            }

            var claimsPrincipal = _autenticacaoService.AutenticarUsuario(usuario);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Json(new { sucesso = true, mensagem = "Login realizado com sucesso!" });
        }

        [HttpGet] 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

    }
}
