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
        private IPlayable.IPlayable board;
        private EPlayerType ePlayerType;

        public Player(IPlayable.IPlayable board, String name)
        {
            this.board = board;
            this.name = name;
        }

        protected string Name { get => name; set => name = value; }
        protected EColorType ColorType { get => colorType; set => colorType = value; }
        protected List<Coin> ListCoins { get => listCoins; set => listCoins = value; }
        protected IPlayable.IPlayable Board { set => board = value; }

    }
}
