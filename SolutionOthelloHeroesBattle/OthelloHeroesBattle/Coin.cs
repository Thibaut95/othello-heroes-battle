using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Coin
    {
        private ECoinType type;
        private String image;
        private EColorType color;

        public Coin(ECoinType type, string image, EColorType color)
        {
            this.type = type;
            this.image = image;
            this.color = color;
        }

        public ECoinType Type { get => type; set => type = value; }
        public string Image { get => image; set => image = value; }
        public EColorType Color { get => color; set => color = value; }
    }
}
