using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Algorithms
{
    public class Shape
    {
        //*******************************CLASS FIELDS**************************************/
        //a list of 4 2D arrays containing our shape in rotations
        public List<byte[,]> rotations;
        public System.Drawing.Color c1 { get; set; }
        public System.Drawing.Color c2 { get; set; }
        //*******************************CLASS METHODS*************************************/
        //create a shape class using byte array passed in as arguments (also finds rotations)
        public Shape(byte[,] map, System.Drawing.Color c1, System.Drawing.Color c2)
        {
            rotations = new List<byte[,]>();
            rotations.Add((byte[,])map.Clone());
            findAllRotations();
            this.c1 = c1;
            this.c2 = c2;
        }
        //finds 3 other rotations based on the first element from rotations list
        //ex. uniqueList = shapeList.Distinct(new shape_comparer()).ToList();
        private void findAllRotations()
        {
            byte[,] map = rotations.ElementAt(0);
            byte[,] outputMatrix;
            var x = 0;
            //col 1 becomes row 1 reversed, 90 degree rotation
            outputMatrix = new byte[map.GetLength(1), map.GetLength(0)];
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
            outputMatrix = new  byte[map.GetLength(0), map.GetLength(1)];
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
            outputMatrix = new byte[map.GetLength(1), map.GetLength(0)];
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
            byte[,] map = rotations.ElementAt(0);
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

        /// <summary>
        /// Override of Shape's Equals, so that it uses the ShapeComparer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this.GetType() == obj.GetType())
            {
                if (new Shape_comparer().Equals(this, (Shape)obj))
                    return true;
            }
            return false;
        }
    }
}
