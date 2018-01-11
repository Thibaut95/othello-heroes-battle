using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    class GameState
    {
        private const int WEIGHTBORDER = 4;
        private const int WEIGHTCENTER = 1;

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
            //TODO
            return new List<Tuple<int, int>>();
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
            int sizeSide = (int)Math.Sqrt(state.Length);

            for (int i = 0; i < sizeSide; i++)
            {
                for (int j = 0; j < sizeSide; j++)
                {
                    int value = state[i, j];
                    if (value == 0)//white
                    {
                        value = -1;
                    }

                    if (i % (sizeSide - 1) == 0 || j % (sizeSide - 1) == 0)
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

        
    }
}
