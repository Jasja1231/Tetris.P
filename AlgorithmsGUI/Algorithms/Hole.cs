using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Algorithms
{
    public class Hole
    {
        public int MaxHeight { get; private set; }

        public Point coords { get; private set; }
        public Hole(byte[,] map, Point point)
        {
            MaxHeight = map.GetLength(0) > map.GetLength(1) ? map.GetLength(0) : map.GetLength(1);
            this.coords = point;
        }
        public Hole() { }
    }
}
