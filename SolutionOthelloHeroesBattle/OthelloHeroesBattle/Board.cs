using IPlayable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Board : IPlayable.IPlayable
    {
        #region Private members
        int[,] board;
        EStateType eStateType;
        #endregion


        public Board()
        {
            board = new int[8, 8];

        }

        public void SetCoin(EColorType color, int line, int col)
        {
            this.board[line,col] = (int)color;
        }

        public EColorType GetCoin(int line, int col)
        {
            return (EColorType)this.board[line, col];
        }

        public void reset()
        {
            throw new NotImplementedException();
        }

        public int GetBlackScore()
        {
            throw new NotImplementedException();
        }

        public int[,] GetBoard()
        {
            return this.board;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool isWhiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            int score = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (this.board[i, j] == (int)EColorType.white)
                    {
                        score++;
                    }
                }
            }

            return score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            int coinType = this.board[line, column];

            if (coinType == (int)EColorType.free)
            {
                if (isWhite)
                {
                    //vérifier toute les directions
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
