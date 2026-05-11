using System;
using System.Collections.Generic;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.Models;

namespace Projeto_AutoMobile.ViewModels.Empresa
{
    public class EmpresaViewModel
    {
        public int EmpresaId { get; set; }

        public DateTime DataAtual { get; set; }

        public List<Veiculo> Frota { get; set; }

        public List<string> Alarmes { get; set; }

        public EmpresaViewModel()
        { 
            Frota = new List<Veiculo>();
            Alarmes = new List<string>();
        }
    }
}
