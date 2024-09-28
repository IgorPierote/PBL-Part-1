using Microsoft.AspNetCore.Mvc;
using PBL_LP.DAO;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    public class EmpresaController : Controller
    {

        public IActionResult Index()
        {
            EmpresaDAO dao = new EmpresaDAO();
            List<EmpresaViewModel> lista = dao.Listagem();
            return View(lista);
        }
        public IActionResult Create()
        {
            ViewBag.Operacao = "I";
            EmpresaViewModel Empresa = new EmpresaViewModel();

            return View("Form", Empresa);
        }
        public IActionResult Salvar(EmpresaViewModel Empresa, string Operacao)
        {
            try
            {

                EmpresaDAO dao = new EmpresaDAO();
                if (Operacao == "I")
                {
                    dao.Inserir(Empresa);
                }
                else
                {
                    dao.Alterar(Empresa);
                }
                return RedirectToAction("index");

            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Edit(string cnpj)
        {
            try
            {
                ViewBag.Operacao = "A";
                EmpresaDAO dao = new EmpresaDAO();
                EmpresaViewModel Empresa = dao.Consulta(cnpj);
                if (Empresa == null)
                    return RedirectToAction("index");
                else
                    return View("Form", Empresa);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Delete(string cnpj)
        {
            try
            {
                EmpresaDAO dao = new EmpresaDAO();
                dao.Excluir(cnpj);
                return RedirectToAction("index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

    }
}
