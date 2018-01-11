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
        private int[,] board;
        private EStateType eStateType;
        private const int SIZE_TILE = 8;
        #endregion


        public Board()
        {
            board = new int[SIZE_TILE, SIZE_TILE];

        }

        public void SetCoin(EColorType color, int line, int col)
        {
            this.board[line, col] = (int)color;
        }

        public EColorType GetCoin(int line, int col)
        {
            return (EColorType)this.board[line, col];
        }

        public void reset()
        {
            for (int i = 0; i < SIZE_TILE; i++)
            {
                for (int j = 0; j < SIZE_TILE; j++)
                {
                    this.board[i, j] = (int)EColorType.free;
                }
            }
        }

        public int[,] GetBoard()
        {
            return this.board;
        }

        /// <summary>
        /// TODO : Thibaut
        /// IA
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO : Thibaut
        /// IA
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool isWhiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetBlackScore()
        {
            return GetCoinScore(EColorType.black);
        }

        public int GetWhiteScore()
        {
            return GetCoinScore(EColorType.white);
        }

        private int GetCoinScore(EColorType eColorType)
        {
            int score = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (this.board[i, j] == (int)eColorType)
                    {
                        score++;
                    }
                }
            }
            return score;
        }


        public bool IsPlayable(int column, int line, bool isWhite)
        {
            #region Init. variables
            int sourceTile = this.board[line, column];
            int currentTile = -1;
            bool isValid = false;
            #endregion

            //on vérifie déjà si la place est libre
            if (sourceTile != (int)EColorType.free)
            {
                return false;
            }

            // On recherche dans chaque direction
            // x et y represente les 8 directions dans lequels nous allons chercher
            //le cas 0, 0 n'est pas intéressant dans notre cas
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 && y != 0 && InBoardArea(line+x, column+y))
                    {
                        currentTile = this.board[line + x, column + y];

                        if (isWhite && currentTile == (int)EColorType.black ||
                            !isWhite && currentTile == (int)EColorType.white)
                        {
                            int posX = line + x;
                            int posY = column + y;

                            //on a trouvé un coup potentiel. 
                            //Donc on va exploité la direction pour voir si on trouve une pièce de la même couleur
                            while (!isValid)
                            {
                                posX += x;
                                posY += y;

                                if (InBoardArea(posX, posY))
                                {
                                    currentTile = this.board[posX, posY];
                                    if(isWhite && currentTile == (int)EColorType.white || !isWhite && currentTile == (int)EColorType.black)
                                    {
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return isValid;
        }

        public static bool InBoardArea(int column, int line)
        {
            return line >= 0 && line <= 7 && column >= 0 && column <= 7;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
