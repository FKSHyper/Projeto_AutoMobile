using System.ComponentModel.DataAnnotations;


namespace Projeto_AutoMobile.ViewModels.Reserva
{
    public class ReservaViewModel
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data de início obrigatória")]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data de fim obrigatória")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "Selecione um cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Selecione um veículo")]
        public int VeiculoId { get; set; }

        public double PrecoEstimado { get; set; }
    }
}
