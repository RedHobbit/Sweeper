using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Sweeper
{
    public class Square : Button
    {
        public bool IsFlagged { get; set; }
        public bool IsMined { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public static readonly DependencyProperty SquareRevealed =
            DependencyProperty.Register("Revealed", typeof (Boolean), typeof (Square));

        public bool Revealed
        {
            get { return (bool)GetValue(SquareRevealed); }
            set { SetValue(SquareRevealed, value); }
        }

        public static readonly DependencyProperty SquarePressed = 
            DependencyProperty.Register("Pressed", typeof(Boolean), typeof(Square));

        public bool Pressed
        {
            get { return (bool)GetValue(SquarePressed); }
            set { SetValue(SquarePressed, value); }
        }

        public Square()
        {
            Content = "";
            Revealed = false;
            IsMined = false;
            PosX = -1;
            PosY = -1;
        }

        public override string ToString()
        {
            return PosX + " , " + PosY;
        }
    }
}
