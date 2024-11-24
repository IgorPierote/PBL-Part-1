using PBL_LP.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace PBL_LP.DAO
{
    public class SensorDAO : PadraoDAO<SensorViewModel>
    {
        public void Inserir(SensorViewModel sensor)
        {
            HelperDAO.ExecutaProc("spInsert_Sensor", CriaParametros(sensor));
        }


        public void Alterar(SensorViewModel sensor)
        {
            HelperDAO.ExecutaProc("spUpdate_Sensor", CriaParametros(sensor));
        }

        protected override SqlParameter[] CriaParametros(SensorViewModel sensor)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("Id", sensor.Id);
            parametros[1] = new SqlParameter("nome", sensor.Nome);
			parametros[2] = new SqlParameter("tipo", sensor.Tipo);

			if (sensor.ValorDoAluguel == null)
                parametros[3] = new SqlParameter("valorDoAluguel", DBNull.Value); 
            else 
                parametros[3] = new SqlParameter("valorDoAluguel", sensor.ValorDoAluguel);
            return parametros;
        }

        public void Excluir(int id)
        {
            var p = new SqlParameter[]
            {
                 new SqlParameter("id", id),
                 new SqlParameter("tabela", "Sensor")
            };
            HelperDAO.ExecutaProc("spDelete", p);
        }

        protected override SensorViewModel MontaModel(DataRow registro)
        {
            SensorViewModel s = new SensorViewModel();
            s.Id = Convert.ToInt32(registro["id"]);
            s.Nome = registro["Nome"].ToString();
            if (registro.Table.Columns.Contains("Descricao"))
                s.Descricao = registro["Descricao"].ToString();
            s.Tipo = Convert.ToInt32(registro["Tipo"]);
            if (registro["ValorDoAluguel"] != DBNull.Value)
                s.ValorDoAluguel = Convert.ToDouble(registro["ValorDoAluguel"]);
            return s;
        }

        public SensorViewModel Consulta(int id)
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


        public override List<SensorViewModel> Listagem()
        {
            List<SensorViewModel> lista = new List<SensorViewModel>();

            DataTable tabela = HelperDAO.ExecutaProcSelect("Sp_ConsultaSensor", null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }

        protected override void SetTabela()
        {
            Tabela = "Sensor";
        }

    }
}
