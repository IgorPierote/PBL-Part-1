using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBL_LP.DAO
{
    public class EmpresaDAO
    {

        public void Inserir(EmpresaViewModel empresa)
        {
            string sql =
            "INSERT INTO Empresa (CNPJ, NomeDaEmpresa, Responsavel, TelefoneContato) " +
            "VALUES (@cnpj, @nomedaempresa, @responsavel, @telefoneContato)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(empresa));
        }

        public void Alterar(EmpresaViewModel empresa)
        {
            string sql =
            "UPDATE Empresa SET NomeDaEmpresa = @nomedaempresa, " +
            "Responsavel = @responsavel, " +
            "TelefoneContato = @telefoneContato " +
            "WHERE CNPJ = @cnpj";
            HelperDAO.ExecutaSQL(sql, CriaParametros(empresa));
        }

        private SqlParameter[] CriaParametros(EmpresaViewModel empresa)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("cnpj", empresa.CNPJ);
            parametros[1] = new SqlParameter("nomedaempresa", empresa.NomeDaEmpresa);
            parametros[2] = new SqlParameter("responsavel", empresa.NomeDoResponsavel);
            parametros[3] = new SqlParameter("telefoneContato", empresa.Telefone.ToString());
            return parametros;
        }

        public void Excluir(string cnpj)
        {
            string sql = "DELETE FROM Empresa WHERE CNPJ = @cnpj";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("cnpj", cnpj);
            HelperDAO.ExecutaSQL(sql, parametros);
        }

        private EmpresaViewModel MontaEmpresa(DataRow registro)
        {
            EmpresaViewModel e = new EmpresaViewModel();
            e.CNPJ = registro["CNPJ"].ToString();
            e.Telefone = registro["TelefoneContato"].ToString();
            e.NomeDoResponsavel = registro["Responsavel"].ToString();
            e.NomeDaEmpresa= registro["NomeDaEmpresa"].ToString();
            return e;
        }

        public EmpresaViewModel Consulta(string cnpj)
        {
            string sql = "SELECT * from Empresa where CNPJ=@cnpj";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("cnpj", cnpj);
            DataTable tabela = HelperDAO.ExecutaSelect(sql, parametros);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaEmpresa(tabela.Rows[0]);
        }

        public List<EmpresaViewModel> Listagem()
        {
            List<EmpresaViewModel> lista = new List<EmpresaViewModel>();
            string sql = "SELECT * from Empresa";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaEmpresa(registro));

            return lista;
        }
    }
}
