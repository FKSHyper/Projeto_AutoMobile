using System.ComponentModel.DataAnnotations;

namespace Projeto_AutoMobile.Data.Projeto_AutoMobile
{
    public class Mota : Veiculo
    {
        //RegularExpression é como um filtro ou um molde para a propriedade
        //A expressão @"^(50cc|125cc|300cc)$" obriga a que seja 50cc ou 125cc ou 300cc
        //^ -> começa aqui $ -> termina aqui () -> agrupam as opções | -> "ou"
        //@ -> diz ao C# para exatamente o que esta dentro da string ignorando caracteres especiais de escape, como \n.
        [RegularExpression(@"^(50cc|125cc|300cc)$", ErrorMessage = "A cilindrada de uma mota tem de ser obrigatoriamente 50cc, 125cc ou 300cc.")]

        public string Cilindrada { get; set; } = string.Empty;
    }
}
