using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIAG4
{
    class GameState
    {
        private const int WEIGHTBORDER = 4;
        private const int WEIGHTCENTER = 1;
        private const int SIZEBOARD = 8;

        private int color;
        private int[,] state;
        private int weight;

        public GameState(int[,] state, int color)
        {
            this.color = color;
            this.state = state;
            this.weight = GetEvaluation();          
        }
    
        public bool IsFinal()
        {
            foreach (int value in state)
            {
                if(value != color && value != -1)
                {
                    return false;
                }
            }
            return true;
        }

        public GameState ApllyMove(Tuple<int,int> move)
        {
            int[,] newState = state;
            newState[move.Item1, move.Item2] = color;
            int newColor = color - 1;
            if (newColor < 0) newColor = 1;
            return new GameState(newState, newColor);
        }

        public List<Tuple<int, int>> GetAvaibleMove()
        {
            List<Tuple<int, int>> listMove = new List<Tuple<int, int>>();
            for (int i = 0; i < SIZEBOARD; i++)
            {
                for (int j = 0; j < SIZEBOARD; j++)
                {
                    if(IsPlayable(i, j, color == (int)EColorType.white))
                    {
                        listMove.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return listMove;
        }

        public int GetEvaluation()
        {
            int tempWeight = GetWeightColor();

            //TODO fonction renvoyant le nombre de coup possible de chaque couleur

            return tempWeight;
        }

        private int GetWeightColor()
        {         
            int counter = 0;

            for (int i = 0; i < SIZEBOARD; i++)
            {
                for (int j = 0; j < SIZEBOARD; j++)
                {
                    int value = state[i, j];
                    if (value == 0)//white
                    {
                        value = -1;
                    }

                    if (i % (SIZEBOARD - 1) == 0 || j % (SIZEBOARD - 1) == 0)
                    {
                        counter += WEIGHTBORDER * value;
                    }
                    else
                    {
                        counter += WEIGHTCENTER * value;
                    }
                }
            }
            return counter;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            #region Init. variables
            int sourceTile = state[line, column];
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
                    if (x != 0 && y != 0 && Board.InBoardArea(line + x, column + y))
                    {
                        currentTile = state[line + x, column + y];

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

                                if (Board.InBoardArea(posX, posY))
                                {
                                    currentTile = state[posX, posY];
                                    if (isWhite && currentTile == (int)EColorType.white || !isWhite && currentTile == (int)EColorType.black)
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
    }
}
