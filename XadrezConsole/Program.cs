
using Xadrez;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PosicaoXadrex pos = new PosicaoXadrex('c', 7);
            Console.WriteLine(pos);
            Console.WriteLine(pos.ToPosicao());
        }
    }
}
