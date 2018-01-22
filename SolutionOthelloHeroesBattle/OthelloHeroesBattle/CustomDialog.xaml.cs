using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OthelloHeroesBattle
{
    /// <summary>
    /// Logique d'interaction pour CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        ECoinType[] arrayPlayerBrush;
        public CustomDialog(ImageBrush brush)
        {
            InitializeComponent();
            winner.Source = brush.ImageSource;
        }

        public CustomDialog(ImageBrush brush, ref ECoinType[] arrayPlayerBrush, ref int timer, ref int score) : this(brush)
        {
            this.arrayPlayerBrush = arrayPlayerBrush;
            Timer.Text = "Timer : " + timer + "seconds";
            Score.Text = "Score : " + score;
        }



        private void Button_PlayAgain(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_ChooseOtherHeroes(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ChoosePlayerDialog choosePlayerDialog = new ChoosePlayerDialog(ref this.arrayPlayerBrush);
            choosePlayerDialog.ShowDialog();
            this.Close();

        }
    }
}
