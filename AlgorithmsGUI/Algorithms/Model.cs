using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    public class Model : Tetris.ObserverDP.Subject
    {
        public Model() { 
        
        }

        /// <summary>
        /// All shapes loaded from file.List of tiles (shapes) without any count.
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


        ///NON TESTED
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Tile"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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




        public bool LoadFromFile(string p)
        {
            bool loaded = true; //status of loading from file

            try {
               string[] content = System.IO.File.ReadAllLines(p);
               var Tiles = FileReader.GetBricksFromFile(content);
               ConstructShapes(Tiles);
            }
            catch {
                loaded = false;
            }

            return loaded;
        }


        private void ConstructShapes(List<byte[,]> Tiles)
        {
            Random random = new Random();
            //clead list of previoustly loaded shapes
            Shapes.Clear();
            foreach (byte[,] tile in Tiles)
            {
                int r = random.Next(100, 220);
                int g = random.Next(100, 220);
                int b = random.Next(100, 220);

                //create and add colors for shapes, add shape to list.
                Shapes.Add(new Shape(tile,System.Drawing.Color.FromArgb(0,r,g,b),System.Drawing.Color.FromArgb(0,r+10,g+10,b+10)));
            }
            
        }


        public void ApplyShapes(List<Controls.TileControl> list)
        {
            ShapesInfoListWrapper s = new ShapesInfoListWrapper();
            s.BuildList(list);
        }
    }
}
