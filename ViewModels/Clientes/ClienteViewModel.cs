using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.ViewModels.Clientes
{
    public class ClienteViewModel
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "NIF obrigatório")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O NIF tem de ter 9 dígitos")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "Carta de condução obrigatória")]
        public string CartaConducao { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        public string Telemovel { get; set; }
    }
}
