using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public enum TipoCaixa
    {
        Manual,
        [Display(Name = "Automática")]
        Automatica
    }

    public class Carro : Veiculo
    {
        [RegularExpression(@"^(3|5)$", ErrorMessage = "O número de portas de um carro tem de ser obrigatoriamente 3 ou 5.")]
        public int NrPortas { get; set; }

        public TipoCaixa Caixa { get; set; }
    }
}