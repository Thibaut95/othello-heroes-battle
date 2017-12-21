using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Board : IPlayable.IPlayable
    {
        public Board()
        {
            throw new NotImplementedException();
        }

        public void SetCoin(EColorType color, int line, int col)
        {
            throw new NotImplementedException();
        }

        public EColorType GetCoin(int line, int col)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            throw new NotImplementedException();
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
