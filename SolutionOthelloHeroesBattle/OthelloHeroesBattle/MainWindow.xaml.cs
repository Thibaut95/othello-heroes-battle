using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

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
        private int countEmptyCell; // mean that the grid is full
        private int skipTurn; // skipTurn = 1 mean one player can't move, skipTurn = 2 mean the both player can't move... So game ended
        private ImageBrush brushWhite;
        private ImageBrush brushBlack;
        private ImageBrush brushWhitePlayable;
        private ImageBrush brushBlackPlayable;
        private ImageBrush brushMarvel;
        private ImageBrush brushDC;
        private Player playerWhite;
        private Player playerBlack;
        private Binding bindingWhite;
        private Binding bindingBlack;
        private Binding bindingWhiteTimer;
        private Binding bindingBlackTimer;
        private DispatcherTimer dtClockTime;
        private ImageBrush brushMarvelDC;
        private ImageBrush brushWallpaper;
        private ImageBrush brushWinner;
        #endregion

        /// <summary>
        /// Consctrutor by default
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //we start a timer
            dtClockTime = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1) //in Hour, Minutes, Second.
            };
            dtClockTime.Tick += DtClockTime_Tick;

            InitGridWithButton();
            LoadAssets();
            BindingPlayer();
            NewGame();

        }

        /// <summary>
        /// Init. the button on the grid
        /// </summary>
        private void InitGridWithButton()
        {
            Style style = this.FindResource("MyButtonStyle") as Style;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button
                    {
                        Style = style
                    };
                    button.Click += Btn_click;
                    button.MouseEnter += BtnMouseEnter;
                    button.MouseLeave += BtnMouseLeave;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    Container.Children.Add(button);
                }
            }
        }

        private void DtClockTime_Tick(object sender, EventArgs e)
        {
            if (isWhiteTurn)
            {
                this.playerWhite.Timer += 1;
                TimerWhite.Text = TimeSpan.FromSeconds(this.playerWhite.Timer).ToString(@"m'm 's's'");
            }
            else
            {
                this.playerBlack.Timer += 1;
                TimerBlack.Text = TimeSpan.FromSeconds(this.playerBlack.Timer).ToString(@"m'm 's's'");
            }
        }

        private void BindingPlayer()
        {
            playerWhite = new Player("Spiderman");
            playerBlack = new Player("Superman");

            bindingWhite = new Binding("Score")
            {
                Source = playerWhite
            };

            bindingBlack = new Binding("Score")
            {
                Source = playerBlack
            };

            bindingWhiteTimer = new Binding("Timer")
            {
                Source = playerWhite
            };

            bindingBlackTimer = new Binding("Timer")
            {
                Source = playerBlack
            };

            ScoreWhite.SetBinding(TextBlock.TextProperty, bindingWhite);
            ScoreBlack.SetBinding(TextBlock.TextProperty, bindingBlack);

            TimerBlack.SetBinding(TextBlock.TextProperty, bindingBlackTimer);
            TimerWhite.SetBinding(TextBlock.TextProperty, bindingWhiteTimer);
        }

        private void LoadAssets()
        {
            brushWhite = ImageManager.GetBrushHeroes(ECoinType.spiderman);
            brushBlack = ImageManager.GetBrushHeroes(ECoinType.superman);

            brushWhitePlayable = brushWhite.Clone();
            brushWhitePlayable.Opacity = 0.3;
            brushBlackPlayable = brushBlack.Clone();
            brushBlackPlayable.Opacity = 0.3;

            brushMarvel = ImageManager.GetBrushImage("marvel_logo.png");
            brushMarvel.Opacity = 0.3;
            brushDC = ImageManager.GetBrushImage("dc_logo.png");
            brushDC.Opacity = 0.3;

            brushMarvelDC = ImageManager.GetBrushImage("marvel_dc_logo.png");
            brushMarvelDC.Opacity = 0.3;

            BtnWhitePlayer.Background = brushWhite;
            BtnBlackPlayer.Background = brushBlack;

            brushWallpaper = ImageManager.GetBrushImage("marvel_logo_1.jpg");
            brushWallpaper.Stretch = Stretch.UniformToFill;
            brushWallpaper.Opacity = 0.1;
            root.Background = Brushes.WhiteSmoke;
        }


        /// <summary>
        /// Start a new game
        /// </summary>
        private void NewGame()
        {
            //we make sure that is the white turn first.
            this.isWhiteTurn = true;


            //update the ui toggle turn
            ToggleTurnUi();

            //reset count
            this.skipTurn = 0;
            this.countEmptyCell = 0;

            //first, init. the board
            this.board = new Board(this.playerWhite.Timer, this.playerBlack.Timer);
            this.board.Reset();

            //reset the player sccore and timer attributes
            this.playerWhite.Reset();
            this.playerBlack.Reset();

            //tools to know if the game is finish
            this.countEmptyCell = 0;

            //we make sure that the content in each button is reset
            UpdateGridGUI();

            //we show the playable move for the player
            ShowThePlayableCell();

            //make sure the game hasn't finish !
            this.GameEnded = false;

            //we start a timer
            dtClockTime.Start();
        }


        private bool ShowThePlayableCell()
        {
            bool isPlayable = false;
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = String.Empty;
                if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)) {
                    if (isWhiteTurn)
                    {
                        button.Background = brushWhitePlayable;
                    }
                    else
                    {
                        button.Background = brushBlackPlayable;
                    }

                    isPlayable = true;
                } else if (this.board[Grid.GetColumn(button), Grid.GetRow(button)] == (int)EColorType.free)
                {
                    button.Background = Brushes.Transparent;
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


                if (this.board.PlayMove(column, row, isWhiteTurn))
                {
                   
                    Console.WriteLine("LEGAL MOVE");
                    //the move is playable so we update the view
                    UpdateGridGUI();
                    //this.board.DebugBoardGame();
                }
                else
                {
                    //the move is not playable...
                    Console.WriteLine("ILLEGAL MOVE");
                    //this.board.DebugBoardGame();
                    return;
                }

                //toggle player
                this.isWhiteTurn = !this.isWhiteTurn;

                CheckForWinner();

                //toggle the ui turn to know who can play
                ToggleTurnUi();

            }
            catch (InvalidCastException ie)
            {
                Console.WriteLine(ie.Message);
            }
        }

        private void CheckForWinner()
        {
            if (!ShowThePlayableCell() && countEmptyCell != 0)
            {
                MessageBox.Show("Pas de coup possible, changement de joueur");
                this.skipTurn += 1;
                this.isWhiteTurn = !this.isWhiteTurn;
                ShowThePlayableCell();
            }
            else
            {
                this.skipTurn = 0;
            }

            // check winner or end game
            if (this.skipTurn >= 2 || countEmptyCell == 0)
            {
                dtClockTime.Stop();
                this.GameEnded = true;
                ShowWinner();

            }
        }

        private void UpdateGridGUI()
        {
            Console.WriteLine("IS_WHITE_TURN " + isWhiteTurn);
            countEmptyCell = 0;

            Container.Children.Cast<Button>().ToList().ForEach(buttonGame =>
            {
                var _column = Grid.GetColumn(buttonGame);
                var _row = Grid.GetRow(buttonGame);
                if (this.board[_column, _row] == (int)EColorType.black)
                {
                    buttonGame.Background = brushBlack;
                }
                else if (this.board[_column, _row] == (int)EColorType.white)
                {
                    buttonGame.Background = brushWhite;
                }
                else
                {
                    buttonGame.Content = String.Empty;
                    countEmptyCell++;
                }
            });

            
            UpdateScore();
        }

        /// <summary>
        /// Update the score of both player and change the background
        /// </summary>
        private void UpdateScore()
        {
            this.playerWhite.Score = this.board.GetWhiteScore();
            this.playerBlack.Score = this.board.GetBlackScore();

            if (this.playerWhite.Score > this.playerBlack.Score)
            {
                Container.Background = brushMarvel;
            }
            else if (this.playerWhite.Score < this.playerBlack.Score)
            {
                Container.Background = brushDC;
            }
            else
            {
                Container.Background = brushMarvelDC;
            }
        }

        private void ToggleTurnUi()
        {
            if (isWhiteTurn)
            {
                PanelWhite.Opacity = 1;
                PanelBlack.Opacity = 0.25;
            }
            else
            {
                PanelWhite.Opacity = 0.25;
                PanelBlack.Opacity = 1;
            }
        }

        /// <summary>
        /// Check if there are a winner
        /// </summary>
        private void ShowWinner()
        {
            this.dtClockTime.Stop();
            if (this.playerWhite.Score > this.playerBlack.Score)
            {
                brushWinner = ImageManager.GetBrushImage("marvel_win.png");
            }
            else if (this.playerWhite.Score < this.playerBlack.Score)
            {
                brushWinner = ImageManager.GetBrushImage("dc_win.jpg");
            }
            else
            {
                brushWinner = ImageManager.GetBrushImage("wallpaper_1.jpg");
            }

            CustomDialog customDialog = new CustomDialog(brushWinner)
            {
                Left = this.Left
            };

            customDialog.ShowDialog();
            this.Show();
            this.NewGame();
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            this.NewGame();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            this.dtClockTime.Stop();

            this.board.IsWhiteTurn = this.isWhiteTurn;
            this.board.TimerBlack = this.playerBlack.Timer;
            this.board.TimerWhite = this.playerWhite.Timer;
            ToolsOthello.SerializeObject(this.board);

            this.dtClockTime.Start();
        }

        private void BtnMouseEnter(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)){
                button.Background = (isWhiteTurn) ? brushWhite : brushBlack;
            }
        }

        private void BtnMouseLeave(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)){
                button.Background = (isWhiteTurn) ? brushWhitePlayable : brushBlackPlayable;
            }
        }

        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Mini(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
            }
                
        }

        private void Button_FullScreen(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Undo(object sender, RoutedEventArgs e)
        {
            if(this.board.UndoMove())
            {
                this.isWhiteTurn ^= true;
                ToggleTurnUi();
                UpdateGridGUI();
                ShowThePlayableCell();
            }
        }

        private void Button_Upload(object sender, RoutedEventArgs e)
        {
            this.dtClockTime.Stop();

            Board uploadBoard = ToolsOthello.DeSerializeObject<Board>();
            if (uploadBoard != null)
            {
                this.board = uploadBoard;
                this.playerBlack.Timer = uploadBoard.TimerBlack;
                this.playerWhite.Timer = uploadBoard.TimerWhite;
                this.isWhiteTurn = uploadBoard.IsWhiteTurn;
                UpdateGridGUI();
                ToggleTurnUi();
                ShowThePlayableCell(); 
            }
            else
            {
                MessageBox.Show("Erreur, fichier pas valide");
            }

            this.dtClockTime.Start();
        }


        /// <summary>
        /// Draggable the window without click in the title bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


    }
}
