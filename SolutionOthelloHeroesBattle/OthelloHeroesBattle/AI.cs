using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IPlayable;

namespace OthelloHeroesBattle
{ 
    class AI
    {
        private const int MAXDEPTH = 4;

        private IPlayable.IPlayable board;
        private Tuple<int, int> bestMove;
        
        public AI(IPlayable.IPlayable board)
        {
            this.board = board;
        }

        public Tuple<int,int> GetNextMove(int color)
        {
            GameState currentState = new GameState(board.GetBoard(), color);
            AlphaBeta(currentState, MAXDEPTH, 1, currentState.GetEvaluation());
            return bestMove;
        }
 
        private int AlphaBeta(GameState gameState, int depth, int minOrMax, int parentValue)
        {
            if(depth == 0 || gameState.IsFinal())
            {
                return gameState.GetEvaluation();
            }
            int bestEvaluation = minOrMax * -int.MaxValue;
            bestMove = null;
            foreach (Tuple<int,int> move in gameState.GetAvaibleMove())
            {
                GameState newState = gameState.ApllyMove(move);
                int tempEvaluation = AlphaBeta(newState, depth - 1, -minOrMax, bestEvaluation);
                if(tempEvaluation * minOrMax > bestEvaluation * minOrMax)
                {
                    bestEvaluation = tempEvaluation;
                    bestMove = move;
                    if(bestEvaluation * minOrMax > parentValue * minOrMax)
                    {
                        break;
                    }
                }
            }
            return bestEvaluation;
        }


        


    }
}
