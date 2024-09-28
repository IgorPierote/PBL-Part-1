using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBL_LP.DAO
{
    public class UsuarioDAO
    {
        public void Inserir(UsuarioViewModel usuario)
        {
            string sql = "INSERT INTO Usuario (CPF, Nome, Email, Senha, Telefone, DataDeNascimento) " +
                         "VALUES (@cpf, @nome, @email, @senha, @telefone, @dataNascimento)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(usuario));
        }

        public void Alterar(UsuarioViewModel usuario)
        {
            string sql = "UPDATE Usuario SET Nome = @nome, Email = @email, " +
                         "Senha = @senha, Telefone = @telefone, DataDeNascimento = @dataNascimento " +
                         "WHERE CPF = @cpf";
            HelperDAO.ExecutaSQL(sql, CriaParametros(usuario));
        }

        private SqlParameter[] CriaParametros(UsuarioViewModel usuario)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("cpf", usuario.CPF);
            parametros[1] = new SqlParameter("nome", usuario.Nome);
            parametros[2] = new SqlParameter("email", usuario.Email);
            parametros[3] = new SqlParameter("senha", usuario.Senha);
            parametros[4] = new SqlParameter("telefone", usuario.Telefone);
            parametros[5] = new SqlParameter("dataNascimento", usuario.DataDeNascimento);
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
                CPF = registro["CPF"].ToString(),
                Nome = registro["Nome"].ToString(),
                Email = registro["Email"].ToString(),
                Senha = registro["Senha"].ToString(),
                Telefone = registro["Telefone"].ToString(),
                DataDeNascimento = Convert.ToDateTime(registro["DataDeNascimento"])
            };
            return u;
        }

        public UsuarioViewModel Consulta(string cpf)
        {
            string sql = "SELECT * FROM Usuario WHERE CPF = @cpf";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("cpf", cpf);
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
    }
}