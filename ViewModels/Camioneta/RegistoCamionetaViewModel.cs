using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.ViewModels.Camioneta
{
    public class RegistoCamionetaViewModel
    {
        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [RegularExpression(@"^([A-Z]{2}|[0-9]{2})-([A-Z]{2}|[0-9]{2})-([A-Z]{2}|[0-9]{2})$",
        ErrorMessage = "Formato inválido. A matricula deve ter um dos seguintes formatos: AA-00-11, 00-AA-11, 11-00-AA.")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O preço por dia é obrigatório.")]
        [Display(Name = "Preço por Dia")]
        public decimal PrecoDia { get; set; }

        [Required(ErrorMessage = "O número de eixos é obrigatório.")]
        [Range(2, 3, ErrorMessage = "O número de eixos de uma camioneta tem de ser 2 ou 3.")]
        [Display(Name = "Número de Eixos")]
        public int NumeroEixos { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O número máximo de passageiros é obrigatório.")]
        [Display(Name = "Máximo de Passageiros")]
        public int NumeroMaximoPassageiros { get; set; }
    }
}
