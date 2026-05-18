using System.ComponentModel.DataAnnotations;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.ViewModels.Carro
{
    public class CarroViewModel
    {
        private double _precoDia;

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
        [RegularExpression(@"^[0-9]{1,6}(\.[0-9]{1,2})?$", ErrorMessage = "O número de portas de um carro tem de ser obrigatoriamente 3 ou 5.")]
        [Display(Name = "Preço por Dia")]

        public double PrecoDia { get { return _precoDia; } set { _precoDia = Math.Round(value, 2); } }
        public EstadoVeiculo Estado { get; set; }

        [Required(ErrorMessage = "O número de portas é obrigatório.")]
        [RegularExpression(@"^(3|5)$", ErrorMessage = "O número de portas de um carro tem de ser obrigatoriamente 3 ou 5.")]
        [Display(Name = "Número de Portas")]
        public int NrPortas { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de caixa.")]
        [Display(Name = "Tipo de Caixa")]
        public TipoCaixa Caixa { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Disponibilidade")]
        public DateTime? DataDisponibilidade { get; set; }
    }
}