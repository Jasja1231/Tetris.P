using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    public class AlgorithmMain : Tetris.ObserverDP.Subject
    {
        public AlgorithmMain() { 
        
        }
        /// <summary>
        /// List of tiles (shapes) 
        /// </summary>
        private List<Shape> shapes = new List<Shape>();
        public List<Shape> Shapes
        {
            get { return shapes; }
            private set { shapes = value; }
        }
        /// <summary>
        /// Adds a tile to a list of shapes
        /// </summary>
        /// <param name="s">Shape to be added</param>
        public void AddShapeToList(Shape s){
            this.shapes.Add(s);
        }
        /// <summary>
        /// Backtracking parameter. The user set K variable, constant for now
        /// </summary>
        private int k = 5;
        public int K
        {
            get { return k; }
            private set { k = value; }
        }

        public bool IsTilePlacementValid (byte[,]Map, byte[,]Tile, int x, int y)
        {
            for (int i = x, i2 = 0; i <= x + Tile.GetLength(0) -1; i++, i2++)
            {
                for (int j = Map.GetLength(1) - 1 - y, j2 = Tile.GetLength(1) -1; j >= Map.GetLength(1) - 1 - y - Tile.GetLength(1) -1; j--, j2--)
                {
                    if (Map[i, j] + Tile[i2, j2] > 1)
                        return false;
                }
            }
            return true;
        }




    }
}
