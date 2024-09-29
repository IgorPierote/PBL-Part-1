using Microsoft.AspNetCore.Mvc;
using PBL_LP.DAO;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    public class AluguelController : Controller
    {
        public IActionResult Index()
        {
            AluguelDAO dao = new AluguelDAO();
            List<AluguelViewModel> lista = dao.Listagem();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Operacao = "I";
            AluguelViewModel aluguel = new AluguelViewModel();
            return View("Form", aluguel);
        }

        [HttpPost]
        public IActionResult Salvar(AluguelViewModel aluguel, string Operacao)
        {
            try
            {
                AluguelDAO dao = new AluguelDAO();
                if (Operacao == "I")
                    dao.Inserir(aluguel);
                else
                    dao.Alterar(aluguel);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Edit(int codigoDoAluguel)
        {
            try
            {
                ViewBag.Operacao = "A";
                AluguelDAO dao = new AluguelDAO();
                AluguelViewModel aluguel = dao.Consulta(codigoDoAluguel);
                if (aluguel == null)
                    return RedirectToAction("Index");
                else
                    return View("Form", aluguel);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Delete(int codigoDoAluguel)
        {
            try
            {
                AluguelDAO dao = new AluguelDAO();
                dao.Excluir(codigoDoAluguel);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}