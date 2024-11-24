using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using PBL_LP.Controllers;
using PBL_LP.DAO;
using System.ComponentModel.DataAnnotations;

public class UsuarioController : PadraoController<UsuarioViewModel>
{
    public UsuarioController()
    {
        DAO = new UsuarioDAO();
        GeraProximoId = true;
    }

    public byte[] ConvertImageToByte(IFormFile file)
    {
        if (file != null)
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        else
            return null;
    }

    protected override void ValidaDados(UsuarioViewModel model, string operacao)
    {
        ModelState.Clear();
        UsuarioDAO DAO = new UsuarioDAO();
        // Validação do CPF
        if (string.IsNullOrEmpty(model.CPF))
            ModelState.AddModelError("CPF", "Preencha o CPF.");
        else if (!System.Text.RegularExpressions.Regex.IsMatch(model.CPF, @"^\d{11,14}$"))
            ModelState.AddModelError("CPF", "CPF inválido. Deve conter apenas números e ter entre 11 e 14 dígitos.");

        // Validação do Nome
        if (string.IsNullOrEmpty(model.Nome))
            ModelState.AddModelError("Nome", "Preencha o nome.");
        else if (model.Nome.Length < 2 || model.Nome.Length > 100)
            ModelState.AddModelError("Nome", "O Nome deve ter entre 2 e 100 caracteres.");

        // Validação do Email
        if (string.IsNullOrEmpty(model.Email))
            ModelState.AddModelError("Email", "Preencha o email.");
        else if (!new EmailAddressAttribute().IsValid(model.Email))
            ModelState.AddModelError("Email", "Email inválido.");

        // Validação da Senha
        if (string.IsNullOrEmpty(model.Senha) && operacao == "I")
            ModelState.AddModelError("Senha", "Preencha a senha.");
        else if (model.Senha?.Length < 6)
            ModelState.AddModelError("Senha", "A senha deve ter no mínimo 6 caracteres.");

        // Validação do Telefone
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

        // Validação da Imagem
        if (model.Imagem == null && operacao == "I")
            ModelState.AddModelError("Foto", "Escolha uma foto.");
        else if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
            ModelState.AddModelError("Foto", "A foto deve ter no máximo 2 MB.");

        if (ModelState.IsValid)
        {
            // Na alteração, se não foi informada a imagem, manter a existente
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