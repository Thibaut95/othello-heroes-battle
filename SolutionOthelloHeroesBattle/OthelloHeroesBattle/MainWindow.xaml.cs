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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace OthelloHeroesBattle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region private members
        private Board board;
        private bool isWhiteTurn;
        private bool GameEnded;
        #endregion

        /// <summary>
        /// Consctrutor by default
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();

        }

        /// <summary>
        /// Start a new game
        /// </summary>
        private void NewGame()
        {
            //first, init. the board
            this.board = new Board();
            this.board.Reset();

            //we make sure that is the white turn first.
            this.isWhiteTurn = true;

            //we make sure that the content in each button is reset
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                if((Grid.GetRow(button) == 3 && Grid.GetColumn(button) == 3) || (Grid.GetRow(button) == 4 && Grid.GetColumn(button) == 4))
                {
                    button.Content = "White";
                }
                else if ((Grid.GetRow(button) == 3 && Grid.GetColumn(button) == 4) || (Grid.GetRow(button) == 4 && Grid.GetColumn(button) == 3))
                {
                    button.Content = "Black";
                }
                else
                {
                    button.Content = String.Empty;
                }
            });

            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(0, 0));
            list.Add(new Tuple<int, int>(2, 2));
            list.Add(new Tuple<int, int>(1, 1));
            ShowThePlayableCell(list);

            //make sure the game hasn't finish !
            this.GameEnded = false;
        }

        private void ShowThePlayableCell(List<Tuple<int, int>>cells)
        {
            foreach (var cell in cells)
            {
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    if(Grid.GetRow(button) == cell.Item1 && Grid.GetColumn(button) == cell.Item2){
                        button.Background = Brushes.Red;
                    }

                });
            }
        }

        /// <summary>
        /// Handles a button click event
        /// </summary>
        /// <param name="sender">The button was clicked</param>
        /// <param name="e">the event of the click</param>
        private void Btn_click(object sender, RoutedEventArgs e)
        {
            //is game ended... start a new game 
            if (this.GameEnded)
            {
                this.NewGame();
                return;
            }

            try
            {
                var button = (Button)sender;
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);


                //if the cell is busy do noting...
                if(this.board.IsPlayable(column, row, this.isWhiteTurn))
                {
                    return;
                }

                //update the board
                board[row, column] = this.isWhiteTurn ? (int)EColorType.white : (int)EColorType.black;

                //update the gui
                button.Content = this.isWhiteTurn ? "White" : "Black";

                //toggle player
                this.isWhiteTurn ^= true;

                //check winner
                if (CheckForWinner()) {
                    this.GameEnded = true;
                }
                else
                {
                    //ShowThePlayableCell();
                }
            }
            catch (InvalidCastException ie)
            {
                Console.WriteLine(ie.Message);
            }
        }

        /// <summary>
        /// Check if there are a winner
        /// </summary>
        private bool CheckForWinner()
        {
            return false;
        }
    }
}
