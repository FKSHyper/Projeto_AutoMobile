using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.ViewModels.Cliente
{
    public class ClienteViewModel
    {
    
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "NIF obrigatório")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O NIF tem de ter 9 dígitos")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF só pode conter 9 números")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "Carta de condução obrigatória")]
        [RegularExpression(@"^[A-Z0-9]{5,20}$", ErrorMessage = "Formato de carta inválido")]
        public string CartaConducao { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "O número de telemóvel tem de ter 9 dígitos")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O número de telemóvel só pode conter 9 números")]
        public string Telemovel { get; set; }
    }
}
