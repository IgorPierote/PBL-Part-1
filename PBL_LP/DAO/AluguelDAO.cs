using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBL_LP.DAO
{
    public class AluguelDAO
    {
        public void Inserir(AluguelViewModel aluguel)
        {
            string sql = "INSERT INTO Aluguel (CodigoDoAluguel, CNPJ, CodigoSensor, Quantidade, DataDeInicio, DataDeFinalizacao, Preco) " +
                         "VALUES (@codigoDoAluguel, @cnpj, @codigoSensor, @quantidade, @dataDeInicio, @dataDeFinalizacao, @preco)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(aluguel));
        }

        public void Alterar(AluguelViewModel aluguel)
        {
            string sql = "UPDATE Aluguel SET CNPJ = @cnpj, CodigoSensor = @codigoSensor, " +
                         "Quantidade = @quantidade, DataDeInicio = @dataDeInicio, " +
                         "DataDeFinalizacao = @dataDeFinalizacao, Preco = @preco " +
                         "WHERE CodigoDoAluguel = @codigoDoAluguel";
            HelperDAO.ExecutaSQL(sql, CriaParametros(aluguel));
        }

        private SqlParameter[] CriaParametros(AluguelViewModel aluguel)
        {
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("codigoDoAluguel", aluguel.CodigoDoAluguel);
            parametros[1] = new SqlParameter("cnpj", aluguel.CNPJ);
            parametros[2] = new SqlParameter("codigoSensor", aluguel.CodigoSensor);
            parametros[3] = new SqlParameter("quantidade", aluguel.Quantidade);
            parametros[4] = new SqlParameter("dataDeInicio", aluguel.DataDeInicio);
            parametros[5] = new SqlParameter("dataDeFinalizacao", aluguel.DataDeFinalizacao);
            parametros[6] = new SqlParameter("preco", aluguel.Preco);
            return parametros;
        }

        public void Excluir(int codigoDoAluguel)
        {
            string sql = "DELETE FROM Aluguel WHERE CodigoDoAluguel = @codigoDoAluguel";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("codigoDoAluguel", codigoDoAluguel);
            HelperDAO.ExecutaSQL(sql, parametros);
        }

        private AluguelViewModel MontaAluguel(DataRow registro)
        {
            AluguelViewModel a = new AluguelViewModel
            {
                CodigoDoAluguel = Convert.ToInt32(registro["CodigoDoAluguel"]),
                CNPJ = registro["CNPJ"].ToString(),
                CodigoSensor = Convert.ToInt32(registro["CodigoSensor"]),
                Quantidade = Convert.ToInt32(registro["Quantidade"]),
                DataDeInicio = Convert.ToDateTime(registro["DataDeInicio"]),
                DataDeFinalizacao = Convert.ToDateTime(registro["DataDeFinalizacao"]),
                Preco = Convert.ToDecimal(registro["Preco"])
            };
            return a;
        }

        public AluguelViewModel Consulta(int codigoDoAluguel)
        {
            string sql = "SELECT * FROM Aluguel WHERE CodigoDoAluguel = @codigoDoAluguel";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("codigoDoAluguel", codigoDoAluguel);
            DataTable tabela = HelperDAO.ExecutaSelect(sql, parametros);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaAluguel(tabela.Rows[0]);
        }

        public List<AluguelViewModel> Listagem()
        {
            List<AluguelViewModel> lista = new List<AluguelViewModel>();
            string sql = "SELECT * FROM Aluguel";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaAluguel(registro));

            return lista;
        }

        public int ProximoId()
        {
            string sql = "select isnull(max(CodigoDoAluguel) +1, 1) as 'MAIOR' from Aluguel";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }

    }
}