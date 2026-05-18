using System.ComponentModel.DataAnnotations;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.ViewModels.Camiao
{
    public class CamiaoViewModel
    {
        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [RegularExpression(@"^([A-Z]{2}-[0-9]{2}-[0-9]{2}|[0-9]{2}-[A-Z]{2}-[0-9]{2}|[0-9]{2}-[0-9]{2}-[A-Z]{2})$",
        ErrorMessage = "Formato inválido. A matricula deve ter um dos seguintes formatos: AA-00-11, 00-AA-11, 11-00-AA.")]
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

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public EstadoVeiculo Estado { get; set; }

        [Required(ErrorMessage = "A capacidade de carga é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade de carga deve ser um número positivo.")]
        [Display(Name = "Capacidade de Carga (Kg)")]
        public int MaxCarga { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Data de Disponibilidade")]
        public DateTime? DataDisponibilidade { get; set; }

    }
}
