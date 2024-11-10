using Microsoft.AspNetCore.Mvc;
using PBL_LP.DAO;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autenticar(string email, string senha)
        {
            UsuarioDAO dao = new UsuarioDAO();
            UsuarioViewModel usuario = dao.VerificarLogin(email, senha);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioLogado", usuario.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email ou senha inválidos.");
                return View("Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioLogado");
            return RedirectToAction("Index", "Login");
        }
    }
}