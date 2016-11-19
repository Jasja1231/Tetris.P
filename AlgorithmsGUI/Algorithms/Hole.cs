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
        public List<Hole> findHoles(MainTable Table)
        {
            List<Hole> holes = null;
            List<Point> tmp = null;
            int maxX = 0, minX = Table.Width, minY = Table.Width, maxY = 0;
            byte[,] temp = Table.Table;
            byte[,] tempHole = null;
            for (int i = 0; i < Table.Height; i++)
            {
                for (int j = 0; j < Table.Width; j++)
                {
                    if(temp[j,i] == 0)
                    {
                        FloodFill(tmp, temp, j, i, 1, 0);
                        foreach (Point k in tmp)
                        {
                            if (k.X < minX) minX = Convert.ToInt32(k.X);
                            if (k.X > maxX) maxX = Convert.ToInt32(k.X);
                            if (k.Y < minY) minY = Convert.ToInt32(k.Y);
                            if (k.Y > maxY) maxY = Convert.ToInt32(k.Y);
                        }
                        for (int y = minY, y2 = 0; y < maxY; y++)
                        {
                            for (int x = minX, x2 = 0; x < maxX; x++)
                            {
                                tempHole[x2, y2] = Table.Table[x, y];
                                x2++;
                            }
                            y2++;
                        }
                        holes.Add(new Hole(tempHole, new Point(minX, minY)));
                    }
                    tmp = null;
                    maxX = 0;
                    minX = Table.Width;
                    minY = Table.Height;
                    maxY = 0;
                    tempHole = null;
                }
            }
            return holes;
        }
        private static void FloodFill(List<Point> coord, byte[,] arr, int x, int y, byte fill, byte old)
        {
            if (x > arr.GetLength(0) - 1 || x < 0)
                return;
            if (y > arr.GetLength(1) - 1 || y < 0)
                return;


            if (arr[x, y] == old)
            {
                coord.Add(new Point(x, y));
                arr[x, y] = fill;
                FloodFill(coord, arr, x + 1, y, fill, old);
                FloodFill(coord, arr, x - 1, y, fill, old);
                FloodFill(coord, arr, x, y + 1, fill, old);
                FloodFill(coord, arr, x, y - 1, fill, old);
            }
        }
    }
}
