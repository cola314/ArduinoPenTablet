using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint_with_Arduino.Arduino
{
    public class PointEventArgs : EventArgs
    {
        public int X;
        public int Y;
        public Color Color;

        public PointEventArgs(int x, int y, Color color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
        }
    }
}
