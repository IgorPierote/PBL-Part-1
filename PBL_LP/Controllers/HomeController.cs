using Microsoft.AspNetCore.Mvc;
using PBL_LP.Filters;
using PBL_LP.Models;
using System.Diagnostics;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Sensor()
        {
            return RedirectToAction("Index", "Sensor");
        }

        public IActionResult Empresa()
        {
            return RedirectToAction("Index", "Empresa");
        }

        public IActionResult Usuario()
        {
            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult Aluguel()
        {
            return RedirectToAction("Index", "Aluguel");
        }

        public IActionResult Sobre()
        {
            return RedirectToAction("Index", "Sobre");
        }

        public IActionResult Temperature()
        {
			return RedirectToAction("Index", "Temperature");
		}

        public IActionResult Login()
        {
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}