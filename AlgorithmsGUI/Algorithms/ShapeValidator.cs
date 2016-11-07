using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    public static class ShapeValidator
    {
        /*
        0-tile is valid
        1-tile is hollow
        2-tile is disconnected
        3-tile is empty
        */
        public static int isTileValid(Shape shape)
        {
            byte[,] arr = (byte[,])shape.rotations[0].Clone();
            byte[,] arr2 = (byte[,])shape.rotations[0].Clone();
            byte[,] arr2extended = new byte[arr2.GetLength(0) + 2, arr2.GetLength(1) + 2];

            for (int i = 0, i2 = 1; i < arr2.GetLength(0); i++, i2++)
            {
                for (int j = 0, j2 = 1; j < arr2.GetLength(0); j++, j2++)
                {
                    arr2extended[i2, j2] = arr[i, j];
                }
            }

            var Index0 = CoordinatesOf((byte[,])arr, (byte)0);
            var Index1 = CoordinatesOf((byte[,])arr, (byte)1);

            if (Index1.Item1 != -1 && Index1.Item2 != -1)   //there is a tile
            {
                FloodFill(ref arr, Index1.Item1, Index1.Item2, 0, 1);
            }
            else
                return 3;

            Index1 = CoordinatesOf((byte[,])arr, (byte)1);

            if (Index0.Item1 != -1 && Index0.Item2 != -1)   //there is empty space
            {
                FloodFill(ref arr2extended, Index0.Item1, Index0.Item2, 1, 0);
            }

            Index0 = CoordinatesOf((byte[,])arr2extended, (byte)0);

            if (Index0.Item1 != -1) //hole inside tile
                return 1;
            if (Index1.Item1 != -1) //disconnected
                return 2;

            return 0;


        }

        private static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        private static void FloodFill(ref byte[,] arr, int x, int y, byte fill, byte old)
        {
            if (x > arr.GetLength(0) - 1 || x < 0)
                return;
            if (y > arr.GetLength(1) - 1 || y < 0)
                return;


            if (arr[x, y] == old)
            {
                arr[x, y] = fill;
                FloodFill(ref arr, x + 1, y, fill, old);
                FloodFill(ref arr, x - 1, y, fill, old);
                FloodFill(ref arr, x, y + 1, fill, old);
                FloodFill(ref arr, x, y - 1, fill, old);
            }
        }

    }
}
