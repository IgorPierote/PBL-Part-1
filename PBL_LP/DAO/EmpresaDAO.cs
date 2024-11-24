using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBL_LP.DAO
{
	public class EmpresaDAO : PadraoDAO<EmpresaViewModel>
	{

		public void Inserir(EmpresaViewModel empresa)
		{
			HelperDAO.ExecutaProc("spInsert_Empresa", CriaParametros(empresa));
		}


		public void Alterar(EmpresaViewModel empresa)
		{
			HelperDAO.ExecutaProc("spUpdate_Empresa", CriaParametros(empresa));
		}

		protected override SqlParameter[] CriaParametros(EmpresaViewModel empresa)
		{
			SqlParameter[] parametros = new SqlParameter[5];
			parametros[0] = new SqlParameter("cnpj", empresa.CNPJ);
			parametros[1] = new SqlParameter("nomedaempresa", empresa.NomeDaEmpresa);
			parametros[2] = new SqlParameter("responsavel", empresa.NomeDoResponsavel);
			parametros[3] = new SqlParameter("telefoneContato", empresa.Telefone.ToString());
			parametros[4] = new SqlParameter("id", empresa.Id);


			return parametros;

		}

		public void Excluir(int id)
		{
			var p = new SqlParameter[]
			{
				 new SqlParameter("id", id),
				 new SqlParameter("tabela", "Empresa")
			};

			HelperDAO.ExecutaProc("spDelete", p);
		}

		protected override EmpresaViewModel MontaModel(DataRow registro)
		{
			EmpresaViewModel e = new EmpresaViewModel();
			e.Id = Convert.ToInt32(registro["id"]);
			e.CNPJ = registro["CNPJ"].ToString();
			e.Telefone = registro["TelefoneContato"].ToString();
			e.NomeDoResponsavel = registro["Responsavel"].ToString();
			e.NomeDaEmpresa = registro["NomeDaEmpresa"].ToString();
			return e;
		}

		public EmpresaViewModel ConsultaCNPJ(string cnpj)
		{
			var p = new SqlParameter[]
			{
		new SqlParameter("cnpj", cnpj)
			};

			DataTable tabela = HelperDAO.ExecutaProcSelect("spConsultaCNPJ", p);

			if (tabela.Rows.Count == 0)
				return null;
			else
				return MontaModel(tabela.Rows[0]);
		}

		public List<EmpresaViewModel> Listagem()
		{
			List<EmpresaViewModel> lista = new List<EmpresaViewModel>();

			DataTable tabela = HelperDAO.ExecutaProcSelect("spListagem", null);

			foreach (DataRow registro in tabela.Rows)
				lista.Add(MontaModel(registro));
			return lista;
		}

		protected override void SetTabela()
		{
			Tabela = "Empresa";
		}
	}
}
