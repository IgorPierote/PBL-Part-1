namespace PBL_LP.Models
{
    public class SensorViewModel:PadraoViewModel
    {
        public string Nome { get; set; }
        public int Tipo { get; set; }

        public string Descricao {  get; set; }
        public double? ValorDoAluguel { get; set; }
    }
}
