using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIAG4
{
    class AI
    {
        private const int MAXDEPTH = 4;

        private IPlayable.IPlayable board;
        private Tuple<int, int> bestMove = null;

        public AI(IPlayable.IPlayable board)
        {
            this.board = board;
        }

        /// <summary>
        /// Fonction qui retourne le prochain coup de l'IA
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int color)
        {
            GameState currentState = new GameState(board.GetBoard(), color);
            AlphaBeta(currentState, MAXDEPTH, 1, currentState.GetEvaluation());
            return bestMove;
        }

        /// <summary>
        /// Implémentation de l'algorithme alpha-beta
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="depth"></param>
        /// <param name="minOrMax"></param>
        /// <param name="parentValue"></param>
        /// <returns></returns>
        private int AlphaBeta(GameState gameState, int depth, int minOrMax, int parentValue)
        {
            int bestEvaluation = minOrMax * -int.MaxValue;
            List<Tuple<int, int>> avaibleMove = gameState.GetAvaibleMove();
            if (avaibleMove.Count == 0)//Pas de coup jouable
            {
                bestMove = new Tuple<int, int>(-1, -1);
                return 0;
            }
            else
            {
                if (depth == 0 || gameState.IsFinal())
                {
                    return gameState.GetEvaluation();
                }
                foreach (Tuple<int, int> move in avaibleMove)
                {
                    Console.WriteLine("Move : ");
                    GameState newState = gameState.ApllyMove(move);
                    int tempEvaluation = AlphaBeta(newState, depth - 1, -minOrMax, bestEvaluation);
                    if (tempEvaluation * minOrMax > bestEvaluation * minOrMax)
                    {
                        bestEvaluation = tempEvaluation;
                        bestMove = move;
                        if (bestEvaluation * minOrMax > parentValue * minOrMax)
                        {
                            break;
                        }
                    }
                }
            }
            return bestEvaluation;
        }
    }
}
