using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    class shape
    {
        //*******************************CLASS FIELDS**************************************/
        //a list of 4 2D arrays containing our shape in rotations
        public List<bool[,]> rotations;
        //*******************************CLASS METHODS*************************************/
        public shape()
        {
            //TODO: create "original" bool map and add it as first rotation
            //TODO: find all rotations
        }
        //make a copy of the shape passed in as argument
        public shape(bool[,] map)
        {
            rotations = new List<bool[,]>();
            rotations.Add((bool[,])map.Clone());
            findAllRotations();
        }
        //finds 3 other rotations based on the first element from rotations list
        //ex. uniqueList = shapeList.Distinct(new shape_comparer()).ToList();
        public void findAllRotations()
        {
            bool[,] map = rotations.ElementAt(0);
            bool[,] outputMatrix;
            var x = 0;
            //col 1 becomes row 1 reversed, 90 degree rotation
            outputMatrix = new bool[map.GetLength(1), map.GetLength(0)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var y = map.GetLength(1) - 1;
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    outputMatrix[y, x] = map[i, j];
                    y--;
                }
                x++;
            }
            rotations.Add(outputMatrix);
            // reverse y's and x's, 180 degree rotation
            x = 0;
            outputMatrix = new bool[map.GetLength(0), map.GetLength(1)];
            for (int i = map.GetLength(0) - 1; i >= 0; i--)
            {
                var y = 0;
                for (int j = map.GetLength(1) - 1; j >= 0; j--)
                {
                    outputMatrix[x, y] = map[i, j];
                    y++;
                }
                x++;
            }
            rotations.Add(outputMatrix);
            // row 1 becomes col 1 reversed, 270 degree rotation
            outputMatrix = new bool[map.GetLength(1), map.GetLength(0)];
            x = map.GetLength(0) - 1;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var y = 0;
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    outputMatrix[y, x] = map[i, j];
                    y++;
                }
                x--;
            }
            rotations.Add(outputMatrix);
        }
        //concatenate shape to StringBuilder in proper txt format
        public void printToString(StringBuilder sb)
        {
            bool[,] map = rotations.ElementAt(0);
            sb.Append(map.GetLength(0).ToString()).Append(" ").Append(map.GetLength(1).ToString()).Append("\n");
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    sb.Append(map[x, y].ToString()).Append(" ");
                }
                sb.Append("\n");
            }
        }
    }
}
