using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Player
    {
        private string name;
        private EColorType colorType;
        private List<Coin> listCoins;
        private Board board;
        private EPlayerType ePlayerType;

        public Player(Board board)
        {
            this.board = board;
        }

        protected string Name { get => name; set => name = value; }
        protected EColorType ColorType { get => colorType; set => colorType = value; }
        protected List<Coin> ListCoins { get => listCoins; set => listCoins = value; }
        protected Board Board { set => board = value; }

    }
}
