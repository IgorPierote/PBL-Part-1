using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace PBL_LP.DAO
{
    public class SensorDAO
    {
        public void Inserir(SensorViewModel sensor)
        {
            string sql =
            "INSERT INTO Sensor (Codigo, Nome, Tipo, ValorDoAluguel) " +
            "VALUES (@codigo, @nome, @tipo, @valorDoAluguel)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(sensor));
        }

        public void Alterar(SensorViewModel sensor)
        {
            string sql =
            "UPDATE Sensor SET Nome = @nome, " +
            "Tipo = @tipo, " +
            "ValorDoAluguel = @valorDoAluguel " +
            "WHERE Codigo = @codigo";
            HelperDAO.ExecutaSQL(sql, CriaParametros(sensor));
        }

        private SqlParameter[] CriaParametros(SensorViewModel sensor)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("codigo", sensor.Codigo);
            parametros[1] = new SqlParameter("nome", sensor.Nome);
			parametros[2] = new SqlParameter("tipo", sensor.Tipo);
			if (sensor.ValorDoAluguel == null)
                parametros[3] = new SqlParameter("valorDoAluguel", DBNull.Value); 
            else 
                parametros[3] = new SqlParameter("valorDoAluguel", sensor.ValorDoAluguel);
            return parametros;
        }

        public void Excluir(int codigo)
        {
            string sql = "DELETE FROM Sensor WHERE Codigo = @codigo";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("codigo", codigo);
            HelperDAO.ExecutaSQL(sql, parametros);
        }

        private SensorViewModel MontaSensor(DataRow registro)
        {
            SensorViewModel s = new SensorViewModel();
            s.Codigo = Convert.ToInt32(registro["Codigo"]);
            s.Nome = registro["Nome"].ToString();
            s.Descricao = registro["Descricao"].ToString();
            if (registro["ValorDoAluguel"] != DBNull.Value)
                s.ValorDoAluguel = Convert.ToDouble(registro["ValorDoAluguel"]);
            return s;
        }

        public SensorViewModel Consulta(int codigo)
        {
            string sql = "SELECT Sensor.Codigo, Nome, Descricao,ValorDoAluguel FROM Sensor inner join TipoSensor on Tipo= TipoSensor.Codigo WHERE Sensor.Codigo = @codigo";
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("codigo", codigo);
            DataTable tabela = HelperDAO.ExecutaSelect(sql, parametros);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaSensor(tabela.Rows[0]);
        }

        public List<SensorViewModel> Listagem()
        {
            List<SensorViewModel> lista = new List<SensorViewModel>();
            string sql = "SELECT Sensor.id, Nome, Descricao,ValorDoAluguel FROM Sensor inner join TipoSensor on Sensor.Tipo= TipoSensor.id ORDER BY Sensor.id";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaSensor(registro));

            return lista;
        }

        public int ProximoId()
        {
            string sql = "select isnull(max(Codigo) +1, 1) as 'MAIOR' from sensor";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }

    }
}
