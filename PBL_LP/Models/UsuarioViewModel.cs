using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Importante para IFormFile

namespace PBL_LP.Models
{
    public class UsuarioViewModel: PadraoViewModel
    {
		public int Id { get; set; }
        public string CPF { get; set; } 

        public string Nome { get; set; } 
        public string Email { get; set; } 

        public string Senha { get; set; }

        public string Telefone { get; set; }

        public DateTime? DataDeNascimento { get; set; }

        public IFormFile Imagem { get; set; }

		public byte[] ImagemEmByte { get; set; }
		/// <summary>
		/// Imagem usada para ser enviada ao form no formato para ser exibida
		/// </summary>
		public string ImagemEmBase64
		{
			get
			{
				if (ImagemEmByte != null)
					return Convert.ToBase64String(ImagemEmByte);
				else
					return string.Empty;
			}
		}
	}
}