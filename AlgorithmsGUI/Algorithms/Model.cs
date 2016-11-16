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

        private List<MainTable> MainTablesList = new List<MainTable>();

        /// <summary>
        /// List of  valid shapes info (count and shape) with list wrapper 
        /// </summary>
        private ShapesInfoListWrapper ShapesInfoList = new ShapesInfoListWrapper();

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

        /// <summary>
        /// Value to show if user ever clicked START with given input.
        /// Used f.e to keep track if we should start the computation when PLAY is clicked or it should be just resumed.
        /// </summary>
        public bool ComputationStarted { get; set; }

        public int TableWidth { get; set; }
        /// <summary>
        /// The height of the heighest shape we have in our shape list
        /// </summary>
        public int MaxShapeHeight { get; set; }


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
               ConstructShapes(Tiles.Item1);
               this.SetMainTableWidth(Tiles.Item2);
            }
            catch {
                loaded = false;
            }

            return loaded;
        }

        /// <summary>
        /// Reads width of the Main tables from the file into property value.
        /// </summary>
        /// <param name="p"></param>
        private void SetMainTableWidth(int p)
        {
            this.TableWidth = p;
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
            this.ShapesInfoList.BuildList(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        internal void StartComputation(int k)
        {
            this.MaxShapeHeight = this.GetMaxShapeHeight();
            this.MainTablesList.Clear();

            //Create k MAIN tables
            for (int i = 0; i < k; i++) {
                MainTable mainTable = new MainTable(i);
                mainTable.Width = this.TableWidth;
                //In the beginning the height of our tables is equal to the height of the talles Shapes (amount ShapesInfoListWrapper)
                mainTable.Height = this.MaxShapeHeight;  
                mainTable.Table = new byte[mainTable.Width,mainTable.Height];

                //Add it to the list of a Main Tables
                this.MainTablesList.Add(mainTable);
            }
            //MAIN LOOP OF THE ALGORITHM
            for(int c = 0 ; c < ShapesInfoList.AvailableShapes.Count;c++)
            {
                //for each MAIN TABLE == from 0 until K
                foreach(MainTable mt in this.MainTablesList)
                {
                    //for each shape
                    for(int i = 0; i < ShapesInfoList.AvailableShapes.Count;i++)
                    {
                        Shape temp = this.ShapesInfoList.GetShapeAt(i);
                        //CREATE THREAD to find its position and start its THREAD_WORK
                    }
                }
                //Copy K best results into our MAIN TABLES
            }
       
	        
        }

        private int GetMaxShapeHeight()
        {
            int maxValue = 0; 
            for (int i = 0; i < ShapesInfoList.AvailableShapes.Count; i++) {
                if (this.ShapesInfoList.GetShapeAt(i).MaxHeight > maxValue)
                    maxValue = this.ShapesInfoList.GetShapeAt(i).MaxHeight;
            }
            return maxValue;
        }
    }
}
