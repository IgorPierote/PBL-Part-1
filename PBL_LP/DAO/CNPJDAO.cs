﻿using PBL_LP.Models;
using System.Data;

namespace PBL_LP.DAO
{
	public class CNPJDAO
	{
		public List<CNPJViewModel> ListaCNPJ()
		{
			List<CNPJViewModel> lista = new List<CNPJViewModel>();
			DataTable tabela = HelperDAO.ExecutaSelect("select CNPJ,NomeDaEmpresa from Empresa", null);
			foreach (DataRow registro in tabela.Rows)
				lista.Add(MontaCNPJ(registro));
			return lista;
		}

		private CNPJViewModel MontaCNPJ(DataRow registro)
		{
			CNPJViewModel c = new CNPJViewModel()
			{
				CNPJ = registro["CNPJ"].ToString(),
				NomeEmpresa = registro["NomeDaEmpresa"].ToString()
			};
			return c;
		}
	}
}
