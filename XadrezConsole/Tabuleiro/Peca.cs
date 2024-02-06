﻿

namespace tabuleiro
{
     class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QntMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca( Cor cor, Tabuleiro tab)
        {
            Posicao = null;
            Cor = cor;
            Tab = tab;
            QntMovimentos = 0;
        }

        public void IncrementarQtdMovimentos()
        {
            QntMovimentos++;
        }

    }
}
