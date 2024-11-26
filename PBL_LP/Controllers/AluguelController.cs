using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_LP.DAO;
using PBL_LP.Filters;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]

    public class AluguelController : PadraoController<AluguelViewModel>
    {
        public AluguelController()
        {
            DAO = new AluguelDAO();
            GeraProximoId = true;
        }

        public override IActionResult Index()
        {
            AluguelDAO aluguelDAO = new AluguelDAO();

            try
            {
                var lista = aluguelDAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected override void PreencheDadosParaView(string operacao, AluguelViewModel model)
        {
            model.DataDeInicio=DateTime.Now;
            model.DataDeFinalizacao=DateTime.Now;
            base.PreencheDadosParaView(operacao, model); // Mantém a lógica base
            PreparaListaCNPJParaCombo(); // Prepara a lista de empresas
            PreparaListaSensoresParaCombo(); // Prepara a lista de sensores
        }

        private void PreparaListaCNPJParaCombo()
        {
            CNPJDAO categoria = new CNPJDAO();
            var categorias = categoria.ListaCNPJ();
            List<SelectListItem> listaCNPJS = new List<SelectListItem>();

            listaCNPJS.Add(new SelectListItem("Selecione uma empresa...", "0"));
            foreach (var c in categorias)
            {
                SelectListItem item = new SelectListItem(c.NomeEmpresa, c.Id.ToString());
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
                SelectListItem item = new SelectListItem(c.Nome, c.Id.ToString());
                listaSensores.Add(item);
            }
            ViewBag.Sensores = listaSensores;
        }
    }
}