using IPlayable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Board : IPlayable.IPlayable
    {
        #region Private members
        private int[,] board;
        private const int SIZE_TILE = 8;

        public event PropertyChangedEventHandler PropertyChanged;

        public int this[int column, int row]       // indexeur
        {
            get { return board[column, row]; }
            set { board[column, row] = (int)value; }
        }


        public int[,] BoardGame { get => board; set => board = value; }
        #endregion


        public Board()
        {
            BoardGame = new int[SIZE_TILE, SIZE_TILE];
        }


        public void SetCoin(EColorType color, int line, int col)
        {
            this.board[col, line] = (int)color;
        }

        public EColorType GetCoin(int line, int col)
        {
            return (EColorType)this.BoardGame[col, line];
        }

        /// <summary>
        /// Reset the board for a new game.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < SIZE_TILE; i++)
            {
                for (int j = 0; j < SIZE_TILE; j++)
                {
                    if((i == 3 && j == 3) || (i == 4 && j == 4))
                    {
                        this.BoardGame[i, j] = (int)EColorType.white;
                    }
                    else if((i == 3 && j == 4) || (i == 4 && j == 3))
                    {
                        this.BoardGame[i, j] = (int)EColorType.black;
                    }
                    else
                    {
                        this.BoardGame[i, j] = (int)EColorType.free;
                    }
                }
            }
        }

        public int[,] GetBoard()
        {
            return this.BoardGame;
        }

        /// <summary>
        /// TODO : Thibaut
        /// IA
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "SHIT IA EVER";
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
            for (int i = 0; i < BoardGame.GetLength(0); i++)
            {
                for (int j = 0; j < BoardGame.GetLength(1); j++)
                {
                    if (this.BoardGame[i, j] == (int)eColorType)
                    {
                        score++;
                    }
                }
            }
            return score;
        }


        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return IsFlip(column, line, isWhite);
        }

        private bool IsFlip(int column, int line, bool isWhite, bool isFlip=false)
        {
            #region Init. variables
            int sourceTile = this.board[column, line];
            int currentTile = -1;
            bool isValid = false;
            bool isLegal = false;
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
                    if ((x != 0 || y != 0) && InBoardArea(column + x, line + y))
                    {
                        currentTile = this.board[column + x, line + y];

                        if (isWhite && currentTile == (int)EColorType.black ||
                            !isWhite && currentTile == (int)EColorType.white)
                        {
                            int posX = column;
                            int posY = line;

                            //on a trouvé un coup potentiel. 
                            //Donc on va exploité la direction pour voir si on trouve une pièce de la même couleur
                            while (!isValid)
                            {
                                posX += x;
                                posY += y;

                                if (InBoardArea(posX, posY))
                                {
                                    currentTile = this.board[posX, posY];
                                    if (currentTile == (int)EColorType.free)
                                    {
                                        isValid = true;
                                    }
                                    else if ((isWhite && currentTile == (int)EColorType.white ) || (!isWhite && currentTile == (int)EColorType.black))
                                    {
                                        if (isFlip)
                                        {
                                            Console.WriteLine("ISFLIPPING");
                                            int color = (isWhite) ? (int)EColorType.white : (int)EColorType.black;
                                            do
                                            {
                                                posX -= x;
                                                posY -= y;
                                                this.board[posX, posY] = color;

                                            } while (posX != column || posY != line);
                                            this.board[column, line] = color;
                                            isValid = true;
                                            isLegal = true;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
       
                                }
                                else
                                {
                                    isValid = true;
                                }

                            }
                            isValid = false;
                        }
                    }
                }
            }
            return isLegal;
        }

        public static bool InBoardArea(int column, int line)
        {
            return line >= 0 && line <= 7 && column >= 0 && column <= 7;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return IsFlip(column, line, isWhite, true);
        }

        public void DebugBoardGame()
        {
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    Console.Write(this.board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
