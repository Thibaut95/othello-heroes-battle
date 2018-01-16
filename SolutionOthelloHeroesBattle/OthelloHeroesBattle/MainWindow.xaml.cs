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
using System.Windows.Resources;
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
        private int countEmptyCell;
        private int skipTurn; // skipTurn = 1 mean one player can't move, skipTurn = 2 mean the both player can't move... So game ended
        private ImageBrush brush;
        private ImageBrush brushSuperman;
        #endregion

        /// <summary>
        /// Consctrutor by default
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ImageManger();

            NewGame();

        }

        private void ImageManger()
        {
            Uri resourceUri = new Uri("images/Spiderman.png", UriKind.Relative);
            Uri resourceSuperman = new Uri("images/Superman.png", UriKind.Relative);

            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);

            brush = new ImageBrush();
            brush.Stretch = Stretch.UniformToFill;
            brush.ImageSource = temp;

            streamInfo = Application.GetResourceStream(resourceSuperman);
            temp = BitmapFrame.Create(streamInfo.Stream);

            brushSuperman = new ImageBrush();
            brushSuperman.Stretch = Stretch.UniformToFill;
            brushSuperman.ImageSource = temp;
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        private void NewGame()
        {
            //first, init. the board
            this.board = new Board();
            this.board.Reset();

            this.countEmptyCell = 0;

            //we make sure that is the white turn first.
            this.isWhiteTurn = true;

            //we make sure that the content in each button is reset
            UpdateGridGUI();

            ShowThePlayableCell();

            //make sure the game hasn't finish !
            this.GameEnded = false;
        }


        private bool ShowThePlayableCell()
        {
            bool isPlayable = false;
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = String.Empty;
                if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), this.isWhiteTurn)) {
                    button.Background = Brushes.Aqua;
                    isPlayable = true;
                } else if (this.board[Grid.GetColumn(button), Grid.GetRow(button)] == (int)EColorType.free)
                {
                    button.Background = Brushes.White;
                }
            });
            return isPlayable;
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

                if(this.board.PlayMove(column, row, isWhiteTurn))
                {
                    Console.WriteLine("LEGAL MOVE");
                    //the move is playable so we update the view
                    UpdateGridGUI();
                    this.board.DebugBoardGame();
                }
                else
                {
                    //the move is not playable...
                    Console.WriteLine("ILLEGAL MOVE");
                    this.board.DebugBoardGame();
                    return;
                }

                //toggle player
                this.isWhiteTurn ^= true;




                if (!ShowThePlayableCell())
                {
                    this.skipTurn += 1;
                    this.isWhiteTurn ^= true;
                }

                // check winner or end game
                if (this.skipTurn == 2 || countEmptyCell == 1)
                {
                    this.GameEnded = true;
                }

            }
            catch (InvalidCastException ie)
            {
                Console.WriteLine(ie.Message);
            }
        }

        private void UpdateGridGUI()
        {
            Container.Children.Cast<Button>().ToList().ForEach(buttonGame =>
            {
                var _column = Grid.GetColumn(buttonGame);
                var _row = Grid.GetRow(buttonGame);
                if (this.board[_column, _row] == (int)EColorType.black)
                {


                    buttonGame.Background = brush;
                }
                else if (this.board[_column, _row] == (int)EColorType.white)
                {
                    buttonGame.Background = brushSuperman;
                }
                else
                {
                    buttonGame.Content = String.Empty;
                    countEmptyCell++;
                }
            });
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
