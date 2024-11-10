using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_LP.DAO;
using PBL_LP.Filters;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]
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
            try
            {
                PreparaListaSensoresParaCombo();
                PreparaListaCNPJParaCombo();

                ViewBag.Operacao = "I";
                AluguelDAO DAO = new AluguelDAO();
                AluguelViewModel aluguel = new AluguelViewModel();
                aluguel.DataDeFinalizacao=DateTime.Now;
                aluguel.DataDeInicio=DateTime.Now;
                aluguel.CodigoDoAluguel = DAO.ProximoId();
                return View("Form", aluguel);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
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
                PreparaListaSensoresParaCombo();
                PreparaListaCNPJParaCombo();
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Edit(int codigoDoAluguel)
        {
            try
            {
                PreparaListaSensoresParaCombo();
                PreparaListaCNPJParaCombo();
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

        private void PreparaListaCNPJParaCombo()
        {
            CNPJDAO categoria = new CNPJDAO();
            var categorias = categoria.ListaCNPJ();
            List<SelectListItem> listaCNPJS = new List<SelectListItem>();

            listaCNPJS.Add(new SelectListItem("Selecione uma empresa...", "0"));
            foreach (var c in categorias)
            {
                SelectListItem item = new SelectListItem(c.NomeEmpresa, c.CNPJ);
                listaCNPJS.Add(item);
            }
            ViewBag.CNPJS = listaCNPJS;
        }

        private void PreparaListaSensoresParaCombo()
        {
            SensorCategoriaDAO categoria = new SensorCategoriaDAO();
            var categorias = categoria.ListaSensor();
            List<SelectListItem> listaSensores = new List<SelectListItem>();

            listaSensores.Add(new SelectListItem("Selecione um sensor...", "0"));
            foreach (var c in categorias)
            {
                SelectListItem item = new SelectListItem(c.Nome, c.Codigo.ToString());
                listaSensores.Add(item);
            }
            ViewBag.Sensores = listaSensores;
        }
    }
}