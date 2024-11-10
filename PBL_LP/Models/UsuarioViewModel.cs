using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Importante para IFormFile

namespace PBL_LP.Models
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres")]
        [RegularExpression(@"^\d{11,14}$", ErrorMessage = "CPF inválido")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O Nome deve ter entre 2 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A Senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [CustomValidation(typeof(UsuarioViewModel), nameof(ValidateDataDeNascimento))]
        public DateTime? DataDeNascimento { get; set; }

        // Nova propriedade para upload de foto
        [Display(Name = "Foto de Perfil")]
        public IFormFile Foto { get; set; }

        // Propriedade para armazenar o caminho da foto
        public string FotoCaminho { get; set; }

        public static ValidationResult ValidateDataDeNascimento(DateTime? dataDeNascimento, ValidationContext context)
        {
            if (dataDeNascimento.HasValue)
            {
                var idade = DateTime.Today.Year - dataDeNascimento.Value.Year;
                if (dataDeNascimento.Value.Date > DateTime.Today.AddYears(-idade)) idade--;

                if (idade < 18)
                {
                    return new ValidationResult("O usuário deve ter pelo menos 18 anos");
                }
            }
            return ValidationResult.Success;
        }
    }
}