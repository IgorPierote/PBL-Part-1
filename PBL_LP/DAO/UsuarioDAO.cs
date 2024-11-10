using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;

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
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("cpf", usuario.CPF);
            parametros[1] = new SqlParameter("nome", usuario.Nome);
            parametros[2] = new SqlParameter("email", usuario.Email);
            parametros[3] = new SqlParameter("senha", usuario.Senha);
            parametros[4] = new SqlParameter("telefone", usuario.Telefone);
            parametros[5] = new SqlParameter("dataNascimento", usuario.DataDeNascimento);

            // Salvar a imagem em uma pasta e definir o caminho
            if (usuario.Foto != null)
            {
                var fileName = Path.GetFileName(usuario.Foto.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/IMG");

                // Certifique-se de que o diretório "IMG" exista
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    usuario.Foto.CopyTo(stream);
                }
                parametros[6] = new SqlParameter("fotoCaminho", $"/IMG/{fileName}");
            }
            else
            {
                parametros[6] = new SqlParameter("fotoCaminho", (object)DBNull.Value);
            }

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
                DataDeNascimento = Convert.ToDateTime(registro["DataDeNascimento"]),
                FotoCaminho = registro["FotoCaminho"].ToString()
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