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
        private const int WEIGHTCORNER = 10;
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
    
        /// <summary>
        /// Retourne vrai si le plateau contient seulement une couleur
        /// </summary>
        /// <returns></returns>
        public bool IsFinal()
        {
            bool containBlack = false;
            bool containWhite = false;
            foreach (int value in state)
            {
                if(value == (int)EColorType.white)
                {
                    containWhite = true;
                }
                else if(value == (int)EColorType.black)
                {
                    containBlack = true;
                }
            }
            return !(containBlack && containWhite);
            
        }

        /// <summary>
        /// Applique un mouvement a l'état courant et retourne un nouvel état
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public GameState ApllyMove(Tuple<int,int> move)
        {
            int[,] newState = (int[,])state.Clone();
            newState[move.Item1, move.Item2] = color;
            int newColor = 0;
            if (color == (int)EColorType.white)
            {
                newColor = (int)EColorType.black;
            }
            else
            {
                newColor = (int)EColorType.white;
            }
            return new GameState(newState, newColor);
        }

        /// <summary>
        /// Retourne une liste de coup possible pour l'état courant
        /// Si on ne passe pas de paramètre la couleur sera la couleur de l'état courant sinon se sera la couleur inverse
        /// </summary>
        /// <param name="selfColor"></param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetAvaibleMove(bool selfColor = true)
        {
            int colorSelected;
            if(selfColor)
            {
                colorSelected = color;
            }
            else
            {
                if(color==(int)EColorType.white)
                {
                    colorSelected = (int)EColorType.black;
                }
                else
                {
                    colorSelected = (int)EColorType.white;
                }

            }
            List<Tuple<int, int>> listMove = new List<Tuple<int, int>>();
            for (int i = 0; i < SIZEBOARD; i++)
            {
                for (int j = 0; j < SIZEBOARD; j++)
                {
                    
                    if (IsPlayable(i, j, colorSelected == (int)EColorType.white))
                    {
                        listMove.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return listMove;
        }

        /// <summary>
        /// Calcul la valeur du noeud
        /// </summary>
        /// <returns></returns>
        public int GetEvaluation()
        {
            int weightColor = GetWeightColor();

            int deltaMove = GetDeltaMove();

            return weightColor*4+deltaMove*2;
        }

        /// <summary>
        /// Cacule la différence de mobilité entre les 2 joueurs
        /// </summary>
        /// <returns></returns>
        private int GetDeltaMove()
        {
            return GetAvaibleMove().Count - GetAvaibleMove(false).Count;
        }

        /// <summary>
        /// Cacule la valeur du plateau
        /// </summary>
        /// <returns></returns>
        private int GetWeightColor()
        {         
            int counter = 0;
            for (int i = 0; i < SIZEBOARD; i++)
            {
                for (int j = 0; j < SIZEBOARD; j++)
                {
                    int value = state[i, j];
                    
                    if(color==(int)EColorType.white)
                    {
                        if (value == (int)EColorType.white)
                        {
                            value = 1;
                        }
                        else if(value == (int)EColorType.black)
                        {
                            value = -1;
                        }
                    }
                    else
                    {
                        if (value == (int)EColorType.white)
                        {
                            value = -1;
                        }
                    }

                    if (i % (SIZEBOARD - 1) == 0 && j % (SIZEBOARD - 1) == 0)
                    {
                        counter += WEIGHTCORNER * value;
                    }
                    else if (i % (SIZEBOARD - 1) == 0 || j % (SIZEBOARD - 1) == 0)
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

        /// <summary>
        /// Retourne vrai si le coup est jouable
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <returns></returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {   
            #region Init. variables
            int sourceTile = state[column, line];
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
                    if ((x != 0 || y != 0) && Board.InBoardArea(column + x, line + y))
                    {
                        currentTile = state[column + x, line + y];

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

                                if (Board.InBoardArea(posX, posY))
                                {
                                    currentTile = state[posX, posY];
                                    if(currentTile==(int)EColorType.free)
                                    {
                                        isValid = true;
                                    }
                                    else if (isWhite && currentTile == (int)EColorType.white || !isWhite && currentTile == (int)EColorType.black)
                                    {
                                        return true;
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
            return false;
        }
    }
}
