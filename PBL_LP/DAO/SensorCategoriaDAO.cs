using PBL_LP.Models;
using System.Data;

namespace PBL_LP.DAO
{
    public class SensorCategoriaDAO
    {
        public List<SensorCategoriaViewModel> ListaSensor()
        {
            List<SensorCategoriaViewModel> lista = new List<SensorCategoriaViewModel>();
            DataTable tabela = HelperDAO.ExecutaSelect("select Codigo,Nome from Sensor", null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaSensorCategoria(registro));
            return lista;
        }

        private SensorCategoriaViewModel MontaSensorCategoria(DataRow registro)
        {
            SensorCategoriaViewModel c = new SensorCategoriaViewModel()
            {
                Codigo = Convert.ToInt32(registro["Codigo"]),
                Nome = registro["Nome"].ToString()
            };
            return c;
        }
    }
}
