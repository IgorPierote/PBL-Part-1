using PBL_LP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para EmailAddressAttribute
using PBL_LP.DAO;
using PBL_LP.Filters;
using Microsoft.AspNetCore.Authorization;

namespace PBL_LP.Controllers
{
	[ServiceFilter(typeof(AutorizacaoFilter))]
	[Route("Usuario")]
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
            GeraProximoId = true;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index(string cpfFilter = null, string nomeFilter = null, string telefoneFilter = null, DateTime? dataNascimentoFilter = null)
        {
            ViewBag.CpfFilter = cpfFilter;
            ViewBag.NomeFilter = nomeFilter;
            ViewBag.TelefoneFilter = telefoneFilter;
            ViewBag.DataNascimentoFilter = dataNascimentoFilter?.ToString("yyyy-MM-dd");

            var usuarios = ((UsuarioDAO)DAO).FiltrarUsuarios(cpfFilter, nomeFilter, telefoneFilter, dataNascimentoFilter);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserList", usuarios);
            }

            return View(usuarios);
        }

        [HttpGet]
		[AllowAnonymous]
		[Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Operacao = "I"; // Indica operação de inserção
            return View("Form", new UsuarioViewModel());
        }

        [HttpPost]
		[AllowAnonymous]
		[Route("Create")]
        public IActionResult Create(UsuarioViewModel model)
        {
            ValidaDados(model, "I");
            if (ModelState.IsValid)
            {
                ((UsuarioDAO)DAO).Inserir(model);
                return RedirectToAction("Index");
            }
            ViewBag.Operacao = "I";
            return View("Form", model);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Operacao = "A"; // Indica operação de alteração
            var model = DAO.Consulta(id);
            return View("Form", model);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(UsuarioViewModel model)
        {
            ValidaDados(model, "A");
            if (ModelState.IsValid)
            {
                ((UsuarioDAO)DAO).Alterar(model);
                return RedirectToAction("Index");
            }
            ViewBag.Operacao = "A";
            return View("Form", model);
        }

        [HttpGet]
        [Route("ConsultaAvancada")]
        public IActionResult ConsultaAvancada()
        {
            return View("ConsultaAvancada");
        }

        [HttpGet]
        [Route("ObtemDadosConsultaAvancada")]
        public IActionResult ObtemDadosConsultaAvancada(string cpfFilter = null, string nomeFilter = null, string telefoneFilter = null, DateTime? dataNascimentoFilter = null)
        {
            try
            {
                var usuarios = ((UsuarioDAO)DAO).FiltrarUsuarios(cpfFilter, nomeFilter, telefoneFilter, dataNascimentoFilter);
                return PartialView("_UserList", usuarios);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            return null;
        }

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            ModelState.Clear();
            if (string.IsNullOrEmpty(model.CPF))
                ModelState.AddModelError("CPF", "Preencha o CPF.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.CPF, @"^\d{11,14}$"))
                ModelState.AddModelError("CPF", "CPF inválido. Deve conter apenas números e ter entre 11 e 14 dígitos.");

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome.");
            else if (model.Nome.Length < 2 || model.Nome.Length > 100)
                ModelState.AddModelError("Nome", "O Nome deve ter entre 2 e 100 caracteres.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o email.");
            else if (!new EmailAddressAttribute().IsValid(model.Email))
                ModelState.AddModelError("Email", "Email inválido.");

            if (string.IsNullOrEmpty(model.Senha) && operacao == "I")
                ModelState.AddModelError("Senha", "Preencha a senha.");
            else if (model.Senha?.Length < 6)
                ModelState.AddModelError("Senha", "A senha deve ter no mínimo 6 caracteres.");

            if (string.IsNullOrEmpty(model.Telefone))
                ModelState.AddModelError("Telefone", "Preencha o telefone.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.Telefone, @"^\+?\d+$"))
                ModelState.AddModelError("Telefone", "Telefone inválido. Deve conter apenas números e, opcionalmente, um prefixo '+'.");

            if (model.DataDeNascimento.HasValue)
            {
                var idade = DateTime.Today.Year - model.DataDeNascimento.Value.Year;
                if (model.DataDeNascimento.Value.Date > DateTime.Today.AddYears(-idade)) idade--;

                if (idade < 18)
                    ModelState.AddModelError("DataDeNascimento", "O usuário deve ter pelo menos 18 anos.");
            }

            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Foto", "Escolha uma foto.");
            else if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Foto", "A foto deve ter no máximo 2 MB.");

            if (ModelState.IsValid)
            {
                if (operacao == "A" && model.Imagem == null)
                {
                    var usuarioExistente = DAO.Consulta(model.Id);
                    model.ImagemEmByte = usuarioExistente.ImagemEmByte;
                }
                else
                {
                    model.ImagemEmByte = ConvertImageToByte(model.Imagem);
                }
            }
        }
    }
}