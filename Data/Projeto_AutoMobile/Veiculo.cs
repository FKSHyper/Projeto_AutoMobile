using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    //um enum é como se fosse um menu fechado, em que so podemos escolher uma destas opcoes ->
    //crio um tipo de dados em que definido as regras que quero que sejam aplicadas
    public enum EstadoVeiculo
    {
        //DataAnnotations. Para o Controller é Disponivel, para o user e como é escrito na Display
        //Na vista inserir dropdown com os valores para o Estado
        //Html.GetEnumSelectList -> inserir na view p ler as displays

        [Display(Name = "Disponível")]
        Disponivel,
        Alugado,
        Reservado,
        [Display(Name = "Em Manutenção")]
        EmManutencao
    }

    public abstract class Veiculo
    {
        private double _precoDia;
        public int Id { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string TipoVeiculo { get; set; } = string.Empty;

        [Precision(8, 2)]
        public double PrecoDia { get { return _precoDia; } set { _precoDia = Math.Round(value, 2); } }
        public EstadoVeiculo Estado { get; set; } //o Estado é do tipo EtsadoVeiculo

        //data prevista para passar ao estado de “disponível”
        public DateTime? DataDisponibilidade { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

    }
}
