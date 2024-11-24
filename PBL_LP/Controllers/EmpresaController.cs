using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using PBL_LP.Controllers;
using PBL_LP.DAO;
using System.ComponentModel.DataAnnotations;

public class EmpresaController : PadraoController<EmpresaViewModel>
{

    public EmpresaController()
    {
        DAO = new EmpresaDAO();
        GeraProximoId = true;
    }
    protected override void ValidaDados(EmpresaViewModel empresaViewModel, string operacao)
    {
        ModelState.Clear();
        EmpresaDAO dao = new EmpresaDAO();
        if (string.IsNullOrEmpty(empresaViewModel.CNPJ))
        {
            ModelState.AddModelError("CNPJ", "Campo obrigatório");
        }
        else if (empresaViewModel.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Length != 14)
            ModelState.AddModelError("CNPJ", "O CNPJ deve conter 14 caracteres");
        else if (operacao == "I" && dao.ConsultaCNPJ(empresaViewModel.CNPJ) != null)
            ModelState.AddModelError("CNPJ", "CNPJ já está em uso.");
        else if (operacao == "A" && dao.ConsultaCNPJ(empresaViewModel.CNPJ) == null)
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

