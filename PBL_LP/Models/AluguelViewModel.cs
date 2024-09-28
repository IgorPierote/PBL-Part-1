using System.ComponentModel.DataAnnotations;

namespace PBL_LP.Models
{
    public class AluguelViewModel
    {
        [Required]
        [Display(Name = "Código do Aluguel")]
        public int CodigoDoAluguel { get; set; }

        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Código do Sensor")]
        public int CodigoSensor { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início")]
        public DateTime DataDeInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Finalização")]
        public DateTime DataDeFinalizacao { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
    }
}