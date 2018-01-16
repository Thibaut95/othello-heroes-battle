using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloHeroesBattle
{
    public class Player : INotifyPropertyChanged
    {
        private string name;
        private int score;

        public Player(String name)
        {
            this.name = name;
            this.score = 0;
        }

        public Player(String name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public int Score {
            get { return score; }
            set {
                score = value;
                OnPropertyChanged("score");
            }
        }
        protected string Name { get => name; set => name = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
