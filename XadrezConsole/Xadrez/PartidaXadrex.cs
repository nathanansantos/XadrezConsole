using tabuleiro;

namespace Xadrez
{
     class PartidaXadrex
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada {  get; private set; }

        public PartidaXadrex()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            terminada = false;
            jogadorAtual = Cor.Branco;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            tab.ColocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrex('c', 1).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrex('c', 2).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrex('d', 2).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrex('e', 2).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrex('e', 1).ToPosicao());
            tab.ColocarPeca(new Rei(Cor.Branco, tab), new PosicaoXadrex('d', 1).ToPosicao());

            tab.ColocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrex('c', 7).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrex('c', 8).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrex('d', 7).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrex('e', 7).ToPosicao());
            tab.ColocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrex('e', 8).ToPosicao());
            tab.ColocarPeca(new Rei(Cor.Preto, tab), new PosicaoXadrex('d', 8).ToPosicao());

        }

    }
}
