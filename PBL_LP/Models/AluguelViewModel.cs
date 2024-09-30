using System.ComponentModel.DataAnnotations;

namespace PBL_LP.Models
{
    public class AluguelViewModel
    {
        [Required(ErrorMessage = "O Código do Aluguel é obrigatório")]
        [Display(Name = "Código do Aluguel")]
        public int CodigoDoAluguel { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido")]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Código do Sensor é obrigatório")]
        [Display(Name = "Código do Sensor")]
        [Range(1, int.MaxValue, ErrorMessage = "O Código do Sensor deve ser um número positivo")]
        public int CodigoSensor { get; set; }

        [Required(ErrorMessage = "A Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser um número positivo")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "A Data de Início é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início")]
        public DateTime DataDeInicio { get; set; }

        [Required(ErrorMessage = "A Data de Finalização é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Finalização")]
        [CustomValidation(typeof(AluguelViewModel), nameof(ValidateDataDeFinalizacao))]
        public DateTime DataDeFinalizacao { get; set; }

        [Required(ErrorMessage = "O Preço é obrigatório")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "O Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        public static ValidationResult ValidateDataDeFinalizacao(DateTime dataDeFinalizacao, ValidationContext context)
        {
            var instance = (AluguelViewModel)context.ObjectInstance;
            if (dataDeFinalizacao <= instance.DataDeInicio)
            {
                return new ValidationResult("A Data de Finalização deve ser posterior à Data de Início");
            }
            return ValidationResult.Success;
        }
    }
}
