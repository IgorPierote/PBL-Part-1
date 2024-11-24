using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;

namespace PBL_LP.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        public void Inserir(UsuarioViewModel usuario)
        {
            HelperDAO.ExecutaProc("spInsert_Usuario", CriaParametros(usuario));
        }


        public void Alterar(UsuarioViewModel usuario)
        {
            HelperDAO.ExecutaProc("spUpdate_Usuario", CriaParametros(usuario));
        }

        protected override SqlParameter[] CriaParametros(UsuarioViewModel usuario)
        {
            object imgByte = usuario.ImagemEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("cpf", usuario.CPF);
            parametros[1] = new SqlParameter("Nome", usuario.Nome);
            parametros[2] = new SqlParameter("email", usuario.Email);
            parametros[3] = new SqlParameter("senha", usuario.Senha);
            parametros[4] = new SqlParameter("telefone", usuario.Telefone);
            parametros[5] = new SqlParameter("datadenascimento", usuario.DataDeNascimento);
            parametros[6] = new SqlParameter("fotoCaminho", imgByte);
			parametros[7] = new SqlParameter("id", usuario.Id);

			return parametros;
        }


        public void Excluir(int id)
        {
            var p = new SqlParameter[]
            {
                 new SqlParameter("id", id),
                 new SqlParameter("tabela", "Usuario")
            };

            HelperDAO.ExecutaProc("spDelete", p);
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
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
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };

            DataTable tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<UsuarioViewModel> Listagem()
        {
            List<UsuarioViewModel> lista = new List<UsuarioViewModel>();

            DataTable tabela = HelperDAO.ExecutaProcSelect("spListagem", null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }

        public int ProximoId()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", "Usuario")
            };

            DataTable tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
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
                return MontaModel(tabela.Rows[0]);
        }

        protected override void SetTabela()
        {
            Tabela = "Usuario";
        }
    }


}
