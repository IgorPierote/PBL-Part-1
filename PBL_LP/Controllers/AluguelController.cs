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