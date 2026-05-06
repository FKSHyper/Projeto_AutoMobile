using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public class Camioneta : Veiculo
    {
        public int MaxPassageiros { get; set; }

        [Range(2, 3, ErrorMessage = "O número de eixos de uma camioneta tem de ser obrigatoriamente 2 ou 3.")]
        public int Eixos { get; set; }
    }
}
