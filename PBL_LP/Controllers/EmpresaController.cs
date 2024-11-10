using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_LP.DAO;
using PBL_LP.Filters;
using PBL_LP.Models;

namespace PBL_LP.Controllers
{
    [ServiceFilter(typeof(AutorizacaoFilter))]
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
				ValidaDados(Empresa, Operacao);
				if (ModelState.IsValid == false)
				{
					ViewBag.Operacao = Operacao;
					return View("Form", Empresa);
				}
				else
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

		public void ValidaDados(EmpresaViewModel empresaViewModel, string operacao)
		{
			ModelState.Clear();
			EmpresaDAO dao = new EmpresaDAO();
			if (string.IsNullOrEmpty(empresaViewModel.CNPJ))
			{
				ModelState.AddModelError("CNPJ", "Campo obrigatório");
			}
			else if (empresaViewModel.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Length != 14)
				ModelState.AddModelError("CNPJ", "O CNPJ deve conter 14 caracteres");
			else if (operacao == "I" && dao.Consulta(empresaViewModel.CNPJ) != null)
				ModelState.AddModelError("CNPJ", "CNPJ já está em uso.");
			else if (operacao == "A" && dao.Consulta(empresaViewModel.CNPJ) == null)
				ModelState.AddModelError("CNPJ", "CNPJ não existe.");

			if (string.IsNullOrEmpty(empresaViewModel.NomeDaEmpresa))
				ModelState.AddModelError("NomeDaEmpresa", "O nome da empresa é obrigatório.");


			// Validação do Telefone
			if (string.IsNullOrEmpty(empresaViewModel.Telefone))
				ModelState.AddModelError("Telefone", "O telefone é obrigatório.");
			else if (!System.Text.RegularExpressions.Regex.IsMatch(empresaViewModel.Telefone, @"^\d{10,11}$"))
				ModelState.AddModelError("Telefone", "O telefone deve conter 10 ou 11 dígitos numéricos.");

			// Validação do Nome do Responsável
			if (string.IsNullOrEmpty(empresaViewModel.NomeDoResponsavel))
				ModelState.AddModelError("NomeDoResponsavel", "O nome do responsável é obrigatório.");
		}
    }
}

