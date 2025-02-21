using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace FinanceFlow.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index(int mes = 0, int ano = 0)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
