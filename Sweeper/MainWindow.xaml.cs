using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Sweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int Mines { get; set; }
        public Game game;

        public ObservableCollection<StackPanel> GameGrid
        {
            get { return game.GameGrid; }
        }

        public MainWindow()
        {
            InitializeComponent();
            {
                GameWindow.DataContext = this;
                Rows = 10;
                Cols = 10;
                Mines = 20;

                game = new Game(Cols, Rows, Mines);

                ResizeGrid();
            }
        }

        private void NewGame()
        {
            if ((Cols * Rows) > Mines)
            {
                game = new Game(Cols, Rows, Mines);
                MineGridSource.ItemsSource = GameGrid;
                ResizeGrid();
            }
        }

        public void ResizeGrid()
        {            
            Width = (Cols * 25) + 40;
            Height = (Rows * 25) + 112;
            MineGrid.Width = Cols * 25;
            MineGrid.Height = Rows * 25;
        }


        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}
