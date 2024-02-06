using tabuleiro; 
using Xadrez;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(Cor.Preto, tab), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(Cor.Preto, tab), new Posicao(1, 9));
                tab.ColocarPeca(new Rei(Cor.Preto, tab), new Posicao(0, 2));
                Tela.ImprimirTabuleiro(tab);

            }
            catch (TabuleiroException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
