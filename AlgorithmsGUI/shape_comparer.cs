using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    class Shape_comparer : IEqualityComparer<Shape>
    {
        public bool Equals(Shape s1, Shape s2)
        {
            byte[,] map = s2.rotations.ElementAt(0);
            foreach (byte[,] rotation in s1.rotations)
            {
                int checker = 0;
                if (rotation.GetLength(1) == map.GetLength(1) && rotation.GetLength(0) == map.GetLength(0))
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        for (int x = 0; x < map.GetLength(0); x++)
                        {
                            if (rotation[x, y] != map[x, y]) break;
                            else if (rotation[x, y] == map[x, y]) checker++;
                        }
                    }
                    if (checker == (rotation.GetLength(0) * rotation.GetLength(1))) return true;
                }
            }
            return false;
        }
        //TODO: figure out good hash function for our shape class
        public int GetHashCode(Shape obj)
        {
            throw new NotImplementedException();
        }
    }
}
