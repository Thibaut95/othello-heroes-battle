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

namespace OthelloHeroesBattle
{
    /// <summary>
    /// Logique d'interaction pour ChoosePlayerDialog.xaml
    /// </summary>
    /// 

    public partial class ChoosePlayerDialog : Window
    {
        private bool isWhiteTurnChoose;
        private ECoinType playerPathBlack;
        private ECoinType playerPathWhite;
        private ECoinType[] playerPath;


        public ChoosePlayerDialog(ref ECoinType[] playerPath)
        {
            InitializeComponent();

            //make sure is white turn to choose
            this.isWhiteTurnChoose = true;

            InitGridWithButton();

            this.playerPath = playerPath;
            this.playerPath[0] = ECoinType.superman; //DC DEFAULT
            this.playerPath[1] = ECoinType.ironman; // MARVEL DEFAULT
        }

        /// <summary>
        /// Init. the button on the grid
        /// </summary>
        private void InitGridWithButton()
        {
            Style style = this.FindResource("MyButtonStyle") as Style;
            int count = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button button = new Button
                    {
                        Style = style
                    };
                    button.Click += Btn_click;
                    ImageBrush brush = ImageManager.GetBrushHeroes(ImageManager.arrayOfTuplesHeroes[count].Item1);
                    button.Background = brush;
                    button.MouseEnter += BtnMouseEnter;
                    button.MouseLeave += BtnMouseLeave;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    Container.Children.Add(button);
                    count++;
                }
            }
        }

        private void BtnMouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Opacity = 1;
            Cursor = Cursors.Arrow;

        }

        private void BtnMouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Opacity = 0.3;
            Cursor = Cursors.Hand;
        }

        private void Btn_click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            int rowFirstChoice = Grid.GetRow(btn);
            int colFirstChoice = Grid.GetColumn(btn);

            if (!isWhiteTurnChoose)
            {
                //we save the second choice and quit the window
                 SaveChoicePlayer(rowFirstChoice, colFirstChoice);

                this.Close();
            }

            //save the first choice 
            SaveChoicePlayer(rowFirstChoice, colFirstChoice);

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                int row = Grid.GetRow(button);
                /**
                 *If the player 1 thake the first row (marvel) so we will disable the marvel choice for the second player 
                 */
                if (row == rowFirstChoice)
                {
                    button.Opacity = 0;
                    button.IsEnabled = false;
                }
            });
            isWhiteTurnChoose ^= true;
            title.Text = "Player 2 : choice";
        }

        private void SaveChoicePlayer(int rowFirstChoice, int colFirstChoice)
        {
            // 0 row is marvel team so white
            if (rowFirstChoice == 0)
            {
                this.playerPath[0] = ImageManager.arrayOfTuplesHeroes[colFirstChoice].Item1;
            }
            else
            {
                this.playerPath[1] = ImageManager.arrayOfTuplesHeroes[4 + colFirstChoice].Item1;
            }
        }
    }
}
