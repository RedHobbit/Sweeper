using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sweeper
{
    public class Game
    {
        private Random rnd;
        public int ColCount { get; set; }
        public int RowCount { get; set; }
        public int MineCount { get; set; }
        public bool bothpressed;
        public ObservableCollection<StackPanel> GameGrid { get; set; }

        /// <summary>
        /// Create a new game of Sweeper
        /// </summary>
        /// <param name="x">Number of columns</param>
        /// <param name="y">Number of rows</param>
        /// <param name="m">Number of mines</param>
        public Game(int x, int y, int m)
        {
            ColCount = x;
            RowCount = y;
            MineCount = m;
            rnd = new Random();
            bothpressed = false;

            GameGrid = CreateGameGrid();

            for (int i = 0; i < MineCount; i++)
            {
                while (!LayMine()) {}
            }
        }

        private ObservableCollection<StackPanel> CreateGameGrid()
        {
            ObservableCollection<StackPanel> grid = new ObservableCollection<StackPanel>();

            for (int i = 0; i < RowCount; i++)
            {
                StackPanel row = new StackPanel();
                row.Orientation = Orientation.Horizontal;


                for (int j = 0; j < ColCount; j++)
                {
                    Square s = new Square();
                    
                    s.Style = Application.Current.FindResource("SquareStyle") as Style;
                    s.PosX = j;
                    s.PosY = i;
                    s.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(LeftMouseUp);
                    s.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(MouseDown);
                    s.MouseRightButtonUp += new MouseButtonEventHandler(RightMouseUp);
                    s.MouseRightButtonDown += new MouseButtonEventHandler(MouseDown);
                    
                    row.Children.Add(s);
                }
                grid.Add(row);
            }



            return grid;
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Square s = sender as Square;

            if (e.RightButton == MouseButtonState.Pressed 
                && e.LeftButton == MouseButtonState.Pressed)
            {
                BothButtonsDown(s);
                return;
            }
        }

        private void LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            Square s = sender as Square;

            if (e.RightButton == MouseButtonState.Pressed)
            {
                BothButtonsUp(s);
                return;
            }

            if (bothpressed)
            {
                
                bothpressed = false;
                return;
            }
            
            if (!s.IsFlagged)
            {
                if (s.IsMined)
                {
                    YouLose();
                }
                else
                {
                    Reveal(s);
                }
            }
        }

        private void RightMouseUp(object sender, MouseButtonEventArgs e)
        {
            Square s = sender as Square;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                BothButtonsUp(s);
                e.Handled = true;
                return;
            }

            if (bothpressed)
            {
                bothpressed = false;
                e.Handled = true;
                return;
            }

            if (!s.Revealed)
            {
                switch (s.IsFlagged)
                {
                    case true:
                        s.IsFlagged = false;
                        s.Content = "";
                        break;
                    case false:
                        s.IsFlagged = true;
                        s.Content = "F";
                        break;
                }
            }

            e.Handled = true;
        }

        private void BothButtonsUp(Square s)
        {
            s.Pressed = false;
            int surroundingMines = 0;
            
            foreach (Square square in GetAdjacent(s))
            {
                if (square.Pressed) square.Pressed = false;
                if (square.IsFlagged) surroundingMines++;
            }

            if (s.Revealed && surroundingMines == (int)s.Content)
            {
                foreach (Square square in GetAdjacent(s))
                {
                    if (!square.Revealed) Reveal(square);
                }
            }
        }

        private void BothButtonsDown(Square s)
        {
            bothpressed = true;
            if (!s.Revealed && !s.IsFlagged) s.Pressed = true;
            foreach (Square square in GetAdjacent(s))
            {
                if(!square.Revealed && !square.IsFlagged) square.Pressed = true;
            }
        }

        private void Reveal(Square s)
        {
            if (!s.Revealed && !s.IsMined && !s.IsFlagged)
            {
                s.Revealed = true;
                
                int val = 0;
                foreach (Square square in GetAdjacent(s))
                {
                    if (square.IsMined) val++;
                }

                if (val > 0) s.Content = val;

                if (val == 0)
                {
                    foreach (Square square in GetAdjacent(s))
                    {
                        Reveal(square);
                    }
                }
            }

            if (s.IsMined && !s.IsFlagged)
            {
                YouLose();
            }
        }

        private bool LayMine()
        {
            int x = rnd.Next(ColCount);
            int y = rnd.Next(RowCount);
            Square s = GameGrid[y].Children[x] as Square;

            if (!s.IsMined)
            {
                s.IsMined = true;
                return true;
            }
            else return false;
        }

        private List<Square> GetAdjacent(Square s)
        {
            List<Square> result = new List<Square>();

            for (int x = s.PosX - 1; x < s.PosX + 2; x++)
            {
                for (int y = s.PosY - 1; y < s.PosY + 2; y++)
                {
                    if (x >= 0 && y >= 0 && x < ColCount && y < RowCount)
                    {
                        result.Add(GameGrid[y].Children[x] as Square);
                    }
                }
            }

            return result;
        }

        private void YouLose()
        {
            foreach(StackPanel panel in GameGrid)
            {
                foreach (Square s in panel.Children)
                {
                    if (s.IsMined) s.Content = "M";
                    s.IsEnabled = false;
                }
            }
        }
    }
}
