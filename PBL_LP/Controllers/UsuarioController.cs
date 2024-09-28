using Microsoft.AspNetCore.Mvc;
using PBL_LP.DAO;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            UsuarioDAO dao = new UsuarioDAO();
            List<UsuarioViewModel> lista = dao.Listagem();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Operacao = "I";
            UsuarioViewModel usuario = new UsuarioViewModel();
            return View("Form", usuario);
        }

        [HttpPost]
        public IActionResult Salvar(UsuarioViewModel usuario, string Operacao)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                if (Operacao == "I")
                    dao.Inserir(usuario);
                else
                    dao.Alterar(usuario);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Edit(string cpf)
        {
            try
            {
                ViewBag.Operacao = "A";
                UsuarioDAO dao = new UsuarioDAO();
                UsuarioViewModel usuario = dao.Consulta(cpf);
                if (usuario == null)
                    return RedirectToAction("Index");
                else
                    return View("Form", usuario);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Delete(string cpf)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                dao.Excluir(cpf);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}