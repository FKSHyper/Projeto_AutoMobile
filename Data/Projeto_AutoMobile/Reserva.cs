namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public class Reserva
    {

        public int Id { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public double PrecoEstimado { get; set; }

        // Foreign Keys
        public int ClienteId { get; set; }

        public int VeiculoId { get; set; }

        // Navigation Properties
        public Cliente Cliente { get; set; }

        public Veiculo Veiculo { get; set; }
        public bool Concluida { get; set; } = false;

    }
}
