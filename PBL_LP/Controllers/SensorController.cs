using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_LP.DAO;
using PBL_LP.Filters;
using PBL_LP.Models;
using System.Reflection;

namespace PBL_LP.Controllers
{

    public class SensorController : PadraoController<SensorViewModel>
    {
        public SensorController()
        {
            DAO = new SensorDAO();
            GeraProximoId = true;
        }

        public override IActionResult Create()
        {
            PreparaTipoSensorJogosParaCombo();
            ViewBag.Operacao = "I";
            SensorViewModel sensor = new SensorViewModel();


            SensorDAO dao = new SensorDAO();
            sensor.Id = dao.ProximoId();

            return View("Form", sensor);
        }

        public override IActionResult Save(SensorViewModel model, string Operacao)
        {
            PreparaTipoSensorJogosParaCombo();

            return base.Save(model, Operacao);
        }

        public override IActionResult Edit(int id)
        {
            PreparaTipoSensorJogosParaCombo();

            return base.Edit(id);
        }

        protected override void ValidaDados(SensorViewModel sensor, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            SensorDAO dao = new SensorDAO();
            if (sensor.Id <= 0)
                ModelState.AddModelError("id", "Id inválido!!!");
            if (string.IsNullOrEmpty(sensor.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome.");
            if (sensor.ValorDoAluguel < 0)
                ModelState.AddModelError("ValorDoAluguel", "Campo obrigatório.");
            if (sensor.Tipo == 0)
                ModelState.AddModelError("Tipo", "Campo obrigatório.");
        }

        private void PreparaTipoSensorJogosParaCombo()
        {
            TipoSensorDAO categoria = new TipoSensorDAO();
            var categorias = categoria.Listagem();
            List<SelectListItem> listaCategorias = new List<SelectListItem>();

            listaCategorias.Add(new SelectListItem("Selecione uma categoria...", "0"));
            foreach (var c in categorias)
            {
                SelectListItem item = new SelectListItem(c.Nome, c.Codigo.ToString());
                listaCategorias.Add(item);
            }
            ViewBag.Tipos = listaCategorias;
        }
    }
}

