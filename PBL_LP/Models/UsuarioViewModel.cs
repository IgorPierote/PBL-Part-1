using System.ComponentModel.DataAnnotations;

public class UsuarioViewModel
{
    [Required]
    public string CPF { get; set; } = string.Empty;

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;

    [Required]
    public string Telefone { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? DataDeNascimento { get; set; }

}