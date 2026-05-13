namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public class Empresa
    {
        public int Id { get; set; }

        // --- Relação com os Veículos --- //

        public virtual ICollection<Veiculo> Frota { get; set; } = new List<Veiculo>();

        // Guarda a data atual para o SIM
        public DateTime DataAtual { get; set; }

        public Empresa()
        {
            // Na inicialização usa a data do sistema
            DataAtual = DateTime.Now;
        }

        // --- Métodos de Gerenciamento de Frota --- //

        public void AdicionarVeiculo(Veiculo veiculo)
        {
            Frota.Add(veiculo);
        }

        public List<Veiculo> ObterVeiculos()
        {
            return Frota.ToList();
        }

        // --- Método para Simulador de Tempo --- //

        public List<string> AvancarDia()
        {
            // Avança a data em um dia
            DataAtual = DataAtual.AddDays(1);

            // Lista para armazenar alarmes 
            List<string> alarmes = new List<string>();

            // Verificar o estado de cada veículo
            foreach (var veiculo in Frota)
            {
                if ((veiculo.Estado == EstadoVeiculo.Alugado || veiculo.Estado == EstadoVeiculo.EmManutencao || veiculo.Estado == EstadoVeiculo.Reservado) && veiculo.DataDisponibilidade.HasValue)
                {
                    if (DataAtual.Date == veiculo.DataDisponibilidade.Value.Date)
                    {
                        alarmes.Add($"ALARME: O veículo com ID {veiculo.Id} terminou o período de '{veiculo.Estado}'.");
                    }
                }
            }

            return alarmes;
        }
        public void RecuarDia()
        {
            // Subtrai um dia à data atual do simulador
            DataAtual = DataAtual.AddDays(-1);
        }
    }
}
