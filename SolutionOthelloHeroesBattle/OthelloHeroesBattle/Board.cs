﻿using IPlayable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OthelloHeroesBattle
{
    [Serializable]
    public class Board
    {
        #region Private members
        private int[,] board;
        private const int SIZE_TILE = 8;
        private int timerWhite;
        private int timerBlack;
        private bool isWhiteTurn;
        private Stack<int[,]> stackBoardStates;

        public event PropertyChangedEventHandler PropertyChanged;

        public int this[int column, int row]       // indexeur
        {
            get { return board[column, row]; }
            set { board[column, row] = (int)value; }
        }


        public int[,] BoardGame { get => board; set => board = value; }
        public int TimerWhite { get => timerWhite; set => timerWhite = value; }
        public int TimerBlack { get => timerBlack; set => timerBlack = value; }
        public bool IsWhiteTurn { get => isWhiteTurn; set => isWhiteTurn = value; }
        #endregion

        /// <summary>
        /// Constructor default
        /// </summary>
        public Board()
        {
            BoardGame = new int[SIZE_TILE, SIZE_TILE];
            stackBoardStates = new Stack<int[,]>();
        }

        /// <summary>
        /// Construtor to init. timer
        /// </summary>
        /// <param name="timerWhite"></param>
        /// <param name="timerBlack"></param>
        public Board(int timerWhite, int timerBlack) : this()
        {
            this.timerWhite = timerWhite;
            this.timerBlack = timerBlack;
        }

        /// <summary>
        /// Set coin in the board
        /// </summary>
        /// <param name="color"></param>
        /// <param name="line"></param>
        /// <param name="col"></param>
        public void SetCoin(EColorType color, int line, int col)
        {
            this.board[col, line] = (int)color;
        }

        /// <summary>
        /// Get coin in the board
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        /// <returns></returns>
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

        /// <summary>
        /// return the board
        /// </summary>
        /// <returns></returns>
        public int[,] GetBoard()
        {
            return this.BoardGame;
        }


        /// <summary>
        /// Return the black score
        /// </summary>
        /// <returns></returns>
        public int GetBlackScore()
        {
            return GetCoinScore(EColorType.black);
        }

        /// <summary>
        /// Get the white score
        /// </summary>
        /// <returns></returns>
        public int GetWhiteScore()
        {
            return GetCoinScore(EColorType.white);
        }


        /// <summary>
        /// return the specific score from a player type
        /// </summary>
        /// <param name="eColorType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Find if is the move is playable and update the board if isFlip = true
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <param name="isFlip"></param>
        /// <returns>boolean</returns>
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
                                            //we save the previous board if the player want to undo the move
                                            this.stackBoardStates.Push(ToolsOthello.CloneArray(this.board));

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

        /// <summary>
        /// Check if the position is in the area
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool InBoardArea(int column, int line)
        {
            return line >= 0 && line <= 7 && column >= 0 && column <= 7;
        }

        /// <summary>
        /// Update the board for a specific move
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <returns></returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            return IsFlip(column, line, isWhite, true);
        }

        /// <summary>
        /// Debug console to show the board
        /// </summary>
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
        
        /// <summary>
        /// Go to the previous state board
        /// </summary>
        /// <returns></returns>
        public bool UndoMove()
        {
            if (this.stackBoardStates.Count > 0)
            {
                this.board = ToolsOthello.CloneArray(this.stackBoardStates.Pop());
                return true;
            }
            return false;
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
