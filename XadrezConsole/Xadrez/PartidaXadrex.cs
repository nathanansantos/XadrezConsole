using System.Collections.Generic;
using tabuleiro;


namespace Xadrez
{
     class PartidaXadrex
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada {  get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaXadrex()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
            jogadorAtual = Cor.Branco;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);

            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao DestinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPeca(T, DestinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao DestinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.ColocarPeca(T, DestinoT);
            }

            // #jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.RetirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if(pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao DestinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(DestinoT);
                T.DecrementarQtdMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao DestinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(DestinoT);
                T.DecrementarQtdMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial en passant
            if(p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }
        }
        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if(EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if(EstaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if(TesteXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }
            
            Peca p = tab.peca(destino);

            // #jogada especial en passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
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
            if (!tab.peca(origem).MovimentoPossivel(destino))
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
                jogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in PecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if(!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();

                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrex(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branco, tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branco, tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branco, tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branco, tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branco, tab, this));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branco, tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branco, tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branco, tab));
            ColocarNovaPeca('a', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branco, tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branco, tab, this));



            ColocarNovaPeca('a', 8, new Torre(Cor.Preto, tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preto, tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preto, tab));
            ColocarNovaPeca('d', 8, new Dama(Cor.Preto, tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preto, tab, this));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preto, tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preto, tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preto, tab));
            ColocarNovaPeca('a', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preto, tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preto, tab, this));

        }

    }
}
