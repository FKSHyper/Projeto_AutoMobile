using System;
using System.Collections.Generic;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.ViewModels.Veiculos
{
    public class VeiculosDisponiveisViewModel
    {
        // A data do teu simulador
        public DateTime DataAtual { get; set; }

        // Guarda o tipo que o utilizador escolheu no filtro
        public string TipoSelecionado { get; set; }

        // Guarda o estado que o utilizador escolheu no filtro
        public string EstadoSelecionado { get; set; }

        // A lista de veículos filtrada que vai aparecer na tabela
        public List<Veiculo> Veiculos { get; set; }

        public VeiculosDisponiveisViewModel()
        {
            Veiculos = new List<Veiculo>();
        }
    }
}