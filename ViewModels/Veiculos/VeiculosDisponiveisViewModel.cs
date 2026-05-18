using System;
using System.Collections.Generic;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.ViewModels.Veiculos
{
    public class VeiculosDisponiveisViewModel
    {
        public DateTime DataAtual { get; set; }

        public string TipoSelecionado { get; set; }

        public string EstadoSelecionado { get; set; }

        public List<Veiculo> Veiculos { get; set; }

        public VeiculosDisponiveisViewModel()
        {
            Veiculos = new List<Veiculo>();
        }
    }
}