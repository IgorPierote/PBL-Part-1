using PBL_LP.Models;
using System.Data;

namespace PBL_LP.DAO
{
    public class TipoSensorDAO
    {
        public List<TipoSensorViewModel> Listagem()
        {
            List<TipoSensorViewModel> lista = new List<TipoSensorViewModel>();
            string sql = "SELECT * from TipoSensor";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaTipoSensor(registro));

            return lista;
        }

        private TipoSensorViewModel MontaTipoSensor(DataRow registro)
        {
            TipoSensorViewModel s = new TipoSensorViewModel();
            s.Codigo = Convert.ToInt32(registro["Id"]);
            s.Nome = registro["Descricao"].ToString();
            return s;
        }
    }
}
