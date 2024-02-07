using tabuleiro;

namespace Xadrez
{
     class PartidaXadrex
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
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

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            turno++;
            MudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if (jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Preto;
            }
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
