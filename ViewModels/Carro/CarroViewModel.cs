using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.ViewModels.Carro
{
    public class CarroViewModel
    {
        [RegularExpression(@"^([A-Z]{2}|[0-9]{2})-([A-Z]{2}|[0-9]{2})-([A-Z]{2}|[0-9]{2})$",
        ErrorMessage = "Formato inválido. A matricula deve ter um dos seguintes formatos: AA-00-11, 00-AA-11, 11-00-AA.")]
        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O preço por dia é obrigatório.")]
        [Display(Name = "Preço por Dia")]
        public decimal PrecoDia { get; set; }
        public EstadoVeiculo Estado { get; set; }

        [Required(ErrorMessage = "O número de portas é obrigatório.")]
        [RegularExpression(@"^(3|5)$", ErrorMessage = "O número de portas de um carro tem de ser obrigatoriamente 3 ou 5.")]
        [Display(Name = "Número de Portas")]
        public int NrPortas { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de caixa.")]
        [Display(Name = "Tipo de Caixa")]
        public TipoCaixa Caixa { get; set; }

        public DateTime? DataDisponibilidade { get; set; }
    }
}