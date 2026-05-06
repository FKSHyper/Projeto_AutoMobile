using System;
using System.Collections.Generic;

namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public class Empresa
    {
        private List<Veiculo> frota;

        public Empresa()
        {
            frota = new List<Veiculo>();
        }

        public void AdicionarVeiculo(Veiculo veiculo)
        {
            frota.Add(veiculo);
        }

        public List<Veiculo> ObterVeiculos()
        {
            return frota;
        }
    }
}
