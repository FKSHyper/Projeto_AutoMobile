using System.ComponentModel.DataAnnotations;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.ViewModels.Mota
{
    public class MotaViewModel
    {
        [RegularExpression(@"^([A-Z]{2}-[0-9]{2}-[0-9]{2}|[0-9]{2}-[A-Z]{2}-[0-9]{2}|[0-9]{2}-[0-9]{2}-[A-Z]{2})$",
        ErrorMessage = "Formato inválido. A matricula deve ter um dos seguintes formatos: AA-00-11, 00-AA-11, 11-00-AA.")]
        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O preço por dia é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço por dia deve ser um valor positivo.")]
        [Display(Name = "Preço por Dia")]
        public double PrecoDia { get; set; }
        public EstadoVeiculo Estado { get; set; }

        [Required(ErrorMessage = "A cilindrada é obrigatória.")]
        [RegularExpression(@"^(50|125|300)$", ErrorMessage = "A cilindrada de uma mota tem de ser obrigatoriamente 50cc, 125cc ou 300cc.")]
        [Display(Name = "Cilindrada (cc)")]
        public string Cilindrada { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data de Disponibilidade")]
        public DateTime? DataDisponibilidade { get; set; }
    }
}