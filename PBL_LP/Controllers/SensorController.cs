using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_LP.DAO;
using PBL_LP.Filters;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]
    public class SensorController : Controller
    {
        public IActionResult Index()
        {
            SensorDAO dao = new SensorDAO();
            List<SensorViewModel> lista = dao.Listagem();
            return View(lista);
        }

        public IActionResult Create()
        {
            PreparaTipoSensorJogosParaCombo();
            ViewBag.Operacao = "I";
            SensorViewModel sensor = new SensorViewModel();


            SensorDAO dao = new SensorDAO();
            sensor.Codigo = dao.ProximoId();

            return View("Form", sensor);
        }
        public IActionResult Salvar(SensorViewModel sensor, string Operacao)
        {
            try
            {
                ValidaDados(sensor, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Opeacao = Operacao;
                    return View("Form", sensor);
                }
                else
                {
                    SensorDAO dao = new SensorDAO();
                    if (Operacao == "I")
                    {
                        dao.Inserir(sensor);
                    }
                    else
                    {
                        dao.Alterar(sensor);
                    }
                    return RedirectToAction("index");
                }
            }
            catch (Exception erro)
            {
                PreparaTipoSensorJogosParaCombo();
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                PreparaTipoSensorJogosParaCombo();
                ViewBag.Operacao = "A";
                SensorDAO dao = new SensorDAO();
                SensorViewModel sensor = dao.Consulta(id);
                if (sensor == null)
                    return RedirectToAction("index");
                else
                    return View("Form", sensor);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                SensorDAO dao = new SensorDAO();
                dao.Excluir(id);
                return RedirectToAction("index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void ValidaDados(SensorViewModel sensor, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            SensorDAO dao = new SensorDAO();
            if (sensor.Codigo <= 0)
                ModelState.AddModelError("Codigo", "Código inválido!");
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

