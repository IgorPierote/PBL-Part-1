﻿using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;

namespace PBL_LP.DAO
{
	public class UsuarioDAO
	{
		public void Inserir(UsuarioViewModel usuario)
		{

			string sql = "INSERT INTO Usuario (CPF, Nome, Email, Senha, Telefone, DataDeNascimento, FotoCaminho) " +
						 "VALUES (@cpf, @nome, @email, @senha, @telefone, @dataNascimento, @fotoCaminho)";
			HelperDAO.ExecutaSQL(sql, CriaParametros(usuario));
		}

		public void Alterar(UsuarioViewModel usuario)
		{
			string sql = "UPDATE Usuario SET Nome = @nome, Email = @email, " +
						 "Senha = @senha, Telefone = @telefone, DataDeNascimento = @dataNascimento, FotoCaminho = @fotoCaminho " +
						 "WHERE CPF = @cpf";
			HelperDAO.ExecutaSQL(sql, CriaParametros(usuario));
		}

		private SqlParameter[] CriaParametros(UsuarioViewModel usuario)
		{
			object imgByte = usuario.ImagemEmByte;
			if (imgByte == null)
				imgByte = DBNull.Value;
			SqlParameter[] parametros =
			{
			 new SqlParameter("cpf", usuario.CPF),
			 new SqlParameter("nome", usuario.Nome),
			 new SqlParameter("email", usuario.Email),
			 new SqlParameter("senha", usuario.Senha),
			 new SqlParameter("telefone", usuario.Telefone),
			 new SqlParameter("dataNascimento", usuario.DataDeNascimento),
			new SqlParameter("fotoCaminho", imgByte)
			};

			return parametros;
		}

		public void Excluir(string cpf)
		{
			string sql = "DELETE FROM Usuario WHERE CPF = @cpf";
			SqlParameter[] parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("cpf", cpf);
			HelperDAO.ExecutaSQL(sql, parametros);
		}


		private UsuarioViewModel MontaUsuario(DataRow registro)
		{
			UsuarioViewModel u = new UsuarioViewModel
			{
				Id = Convert.ToInt16(registro["Id"]),
				CPF = registro["CPF"].ToString(),
				Nome = registro["Nome"].ToString(),
				Email = registro["Email"].ToString(),
				Senha = registro["Senha"].ToString(),
				Telefone = registro["Telefone"].ToString(),
				DataDeNascimento = Convert.ToDateTime(registro["DataDeNascimento"]),
				ImagemEmByte = registro["fotoCaminho"] as byte[]
			};
			return u;
		}

		public UsuarioViewModel Consulta(int id)
		{
			string sql = "SELECT * FROM Usuario WHERE Id = @id";
			SqlParameter[] parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("cpf", id);
			DataTable tabela = HelperDAO.ExecutaSelect(sql, parametros);

			if (tabela.Rows.Count == 0)
				return null;
			else
				return MontaUsuario(tabela.Rows[0]);
		}

		public List<UsuarioViewModel> Listagem()
		{
			List<UsuarioViewModel> lista = new List<UsuarioViewModel>();
			string sql = "SELECT * FROM Usuario";
			DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

			foreach (DataRow registro in tabela.Rows)
				lista.Add(MontaUsuario(registro));

			return lista;
		}

		public UsuarioViewModel VerificarLogin(string email, string senha)
		{
			string sql = "SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha";
			SqlParameter[] parametros = new SqlParameter[2];
			parametros[0] = new SqlParameter("Email", email);
			parametros[1] = new SqlParameter("Senha", senha);
			DataTable tabela = HelperDAO.ExecutaSelect(sql, parametros);

			if (tabela.Rows.Count == 0)
				return null;
			else
				return MontaUsuario(tabela.Rows[0]);
		}
	}
}