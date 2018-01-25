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
using System.Windows.Media.Effects;
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
        private ECoinType[] imageCoinType;
        private Binding bindingWhiteTimer;
        private Binding bindingBlackTimer;
        private DispatcherTimer dtClockTime;
        private ImageBrush brushMarvelDC;
        private ImageBrush brushWallpaper;
        private ImageBrush brushWinner;
        DoubleAnimation daFadeIn = new DoubleAnimation();
        #endregion

        /// <summary>
        /// Consctrutor by default
        /// </summary>
        public MainWindow()
        {
            DoubleAnimation daIn = new DoubleAnimation();

            Welcome welcomeScreen = new Welcome();
            welcomeScreen.ShowDialog();

            imageCoinType = new ECoinType[2];

            ChoosePlayerDialog choosePlayerDialog = new ChoosePlayerDialog(ref imageCoinType);
            choosePlayerDialog.ShowDialog();

            InitializeComponent();

            Container.Background = Brushes.White;

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

        /// <summary>
        /// Manage the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Make all the binding with the "widget"
        /// </summary>
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

        /// <summary>
        /// Load all and instanciate all brush
        /// </summary>
        private void LoadAssets()
        {

            brushWhite = ImageManager.GetBrushHeroes(imageCoinType[0]);
            brushBlack = ImageManager.GetBrushHeroes(imageCoinType[1]);

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

            //we update the assets
            LoadAssets();

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



            //we make sure that the content in each button is reset
            UpdateGridGUI();

            //we show the playable move for the player
            ShowThePlayableCell();

            //make sure the game hasn't finish !
            this.GameEnded = false;

            //we start a timer
            dtClockTime.Start();
        }

        /// <summary>
        /// Show the cell playable
        /// </summary>
        /// <returns></returns>
        private bool ShowThePlayableCell()
        {
            bool isPlayable = false;
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = String.Empty;
                if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)) {
                    if (isWhiteTurn)
                    {
                        AnimateButtonFadeIn(ref button, ref brushWhitePlayable);
                    }
                    else
                    {
                        AnimateButtonFadeIn(ref button, ref brushBlackPlayable);
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

        /// <summary>
        /// Check for any winner
        /// </summary>
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

        /// <summary>
        /// Update the grid and show the right coin
        /// </summary>
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
                    AnimateButtonRotation(ref buttonGame, ref brushBlack);
                    AnimateButtonFadeIn(ref buttonGame, ref brushBlack);
                }
                else if (this.board[_column, _row] == (int)EColorType.white)
                {
                    AnimateButtonRotation(ref buttonGame, ref brushWhite);
                    AnimateButtonFadeIn(ref buttonGame, ref brushWhite);

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
        /// Animation fade in for the button in the grid container
        /// </summary>
        /// <param name="buttonGame"></param>
        /// <param name="brush"></param>
        private void AnimateButtonFadeIn(ref Button buttonGame, ref ImageBrush brush)
        {
            if (!buttonGame.Background.Equals(brush))
            {
                daFadeIn.From = 0;
                daFadeIn.To = 1;
                daFadeIn.Duration = new Duration(TimeSpan.FromSeconds(2));
                buttonGame.Background = brush;
                buttonGame.BeginAnimation(OpacityProperty, daFadeIn);
            }
        }

        /// <summary>
        /// Animation rotation for the button in the grid container
        /// </summary>
        /// <param name="buttonGame"></param>
        /// <param name="brush"></param>
        private void AnimateButtonRotation(ref Button buttonGame, ref ImageBrush brush)
        {
            if (!buttonGame.Background.Equals(brush))
            {
                DoubleAnimation da = new DoubleAnimation
               (
                    0,
                    360,
                    new Duration(TimeSpan.FromSeconds(1))
               );

                RotateTransform rt = new RotateTransform();
                buttonGame.RenderTransform = rt;
                buttonGame.RenderTransformOrigin = new Point(0.5, 0.5);
                buttonGame.BeginAnimation(OpacityProperty, daFadeIn);
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
            }
        }

        /// <summary>
        /// Animation fade in for the background of the grid container
        /// </summary>
        /// <param name="brush"></param>
        private void AnimateBackgroundFadeIn(ref ImageBrush brush)
        {
            if (!Container.Background.Equals(brush))
            {
                daFadeIn.From = 0;
                daFadeIn.To = 0.5;
                daFadeIn.Duration = new Duration(TimeSpan.FromSeconds(2));
                Container.Background = brush;
                brush.BeginAnimation(ImageBrush.OpacityProperty, daFadeIn);
            }
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
                AnimateBackgroundFadeIn(ref brushMarvel);
            }
            else if (this.playerWhite.Score < this.playerBlack.Score)
            {
                AnimateBackgroundFadeIn(ref brushDC);
            }
            else
            {
                AnimateBackgroundFadeIn(ref brushMarvelDC);
            }
        }

        /// <summary>
        /// Update the ui game to show who is turn
        /// </summary>
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
            int timer = 0;
            int score = 0;

            this.dtClockTime.Stop();
            if (this.playerWhite.Score > this.playerBlack.Score)
            {
                brushWinner = brushWhite;
                timer = playerWhite.Timer;
                score = playerWhite.Score;
            }
            else if (this.playerWhite.Score < this.playerBlack.Score)
            {
                brushWinner = brushBlack;
                timer = playerBlack.Timer;
                score = playerBlack.Score;
            }
            else
            {
                brushWinner = ImageManager.GetBrushImage("wallpaper_1.jpg");
            }

            CustomDialog customDialog = new CustomDialog(brushWinner, ref imageCoinType, ref timer, ref score);

            ShowDialogWinner(ref customDialog);
        }

        /// <summary>
        /// Show the dialog window winner with the player who win
        /// </summary>
        /// <param name="customDialog"></param>
        private void ShowDialogWinner(ref CustomDialog customDialog)
        {
            var blur = new BlurEffect();
            var current = this.Background;
            blur.Radius = 10;
            this.Background = new SolidColorBrush(Colors.DarkGray);
            this.Effect = blur;

            customDialog.ShowDialog();

            this.Effect = null;
            this.Background = current;
            this.NewGame();
        }

        /// <summary>
        /// trigger that reset the board ui and run a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            this.NewGame();
        }

        /// <summary>
        /// Save the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            this.dtClockTime.Stop();

            this.board.IsWhiteTurn = this.isWhiteTurn;
            this.board.TimerBlack = this.playerBlack.Timer;
            this.board.TimerWhite = this.playerWhite.Timer;
            ToolsOthello.SerializeObject(this.board);

            this.dtClockTime.Start();
        }

        /// <summary>
        /// Event mouse hover button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMouseEnter(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)){
                button.Background = (isWhiteTurn) ? brushWhite : brushBlack;
            }
        }

        /// <summary>
        /// Event mouse leave button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMouseLeave(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(this.board.IsPlayable(Grid.GetColumn(button), Grid.GetRow(button), isWhiteTurn)){
                button.Background = (isWhiteTurn) ? brushWhitePlayable : brushBlackPlayable;
            }
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Hide the game window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Mini(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Put the game window fullscreen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_FullScreen(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// Go back to the previous move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Upload a old game with the format *.heroes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
