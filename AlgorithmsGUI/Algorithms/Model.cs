using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Algorithms
{
    public class Model : Tetris.ObserverDP.Subject
    {
        public Model()
        {
            threadComp = new ThreadComputation(this);
        }

        private volatile bool playing;
        private int iterLeft;

        /// <summary>
        /// Class for preforming iterations and collecting results without blocking main thread
        /// </summary>
        private ThreadComputation threadComp;

        /// <summary>
        /// Last displayed image sources.
        /// </summary>
        public List<ImageSource> ImageSources = new List<ImageSource>();

        /// <summary>
        /// number of all shapes left to place (NOT unique shapes)
        /// </summary>
        public int RemainingShapes { get; set; }

        /// <summary>
        /// All shapes loaded from file.List of tiles (shapes) without any count.
        /// </summary>
        private List<Shape> shapes = new List<Shape>();

        public List<Shape> AllLoadedShapes
        {
            get { return shapes; }
            private set { shapes = value; }
        }

        public Shape[] ShapesDatabase { get;  set; }
        public int[] ShapeQuantities { get; private set; }

        public List<MainTable> MainTablesList = new List<MainTable>();
  
        public List<Result> BestResults = new List<Result>(10);/////////////////////////////////////////////////////

        /// <summary>
        /// Adds a tile to a list of shapes
        /// </summary>
        /// <param name="s">Shape to be added</param>
        public void AddShapeToList(Shape s)
        {
            this.shapes.Add(s);
        }

        /// <summary>
        /// Backtracking parameter. The user set K variable, constant for now
        /// </summary>
        private int k;
        public int K
        {
            get { return k; }
            set { k = value; }
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
        public bool IsTilePlacementValid(byte[,] Map, byte[,] Tile, int x, int y)
        {
            for (int i = x, i2 = 0; i <= x + Tile.GetLength(0) - 1; i++, i2++)
            {
                for (int j = Map.GetLength(1) - 1 - y, j2 = Tile.GetLength(1) - 1; j >= Map.GetLength(1) - 1 - y - Tile.GetLength(1) - 1; j--, j2--)
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

            try
            {
                string[] content = System.IO.File.ReadAllLines(p);
                var Tiles = FileReader.GetBricksFromFile(content);
                ConstructShapes(Tiles.Item1);
                this.SetMainTableWidth(Tiles.Item2);
            }
            catch
            {
                loaded = false;
            }

            return loaded;
        }

        public void AddBestResults(List<Result> bestResults)
        {
            //check if reset was pressed
            if (ComputationStarted == false)
            {
                MainTablesList.Clear();
                return;
            }
            
            this.BestResults = bestResults;
            var mtlTmp = new List<MainTable>(this.K);
            //update maintables
            for (int i = 0; i < this.K; i++)
            {
                Result r = bestResults.ElementAt(i);
                var newtable = (MainTablesList.ElementAt(r.Kth).UpdateWithResult(r, this));
                newtable.Kth = i;
                mtlTmp.Add(newtable);

            }
            this.MainTablesList = mtlTmp;

            //NOTIFY GUI RESULTS ARE READY
            this.Notify(1);

            //if we are "playing" then preform next iterations, or we still have some iterations to do
            if (playing && RemainingShapes > 0)
            {
                threadComp.preformIteration(this, this.k, MainTablesList);
            }
            else if (RemainingShapes > 0 && iterLeft > 1)
            {
                iterLeft--;
                threadComp.preformIteration(this, this.k, MainTablesList);
            }
            else if (RemainingShapes <= 0)
            {
                MessageBox.Show("==========================\nNo more shapes\n(^_^）o自自o（^_^ ）\nCheers mate!\n==========================");
            }
            else
            {
                MessageBox.Show("==========================\nWaiting for more input! (｡◕‿◕｡)\n==========================");
            }

            /******DEBUG***********************************************************
            foreach (MainTable saaaaaaaa in this.MainTablesList)
            {
                Console.Out.WriteLine("MAIN TABLE" + saaaaaaaa.Kth.ToString());
                for (int y = saaaaaaaa.Table.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int x = 0; x < saaaaaaaa.Table.GetLength(0); x++)
                    {
                        Console.Out.Write(saaaaaaaa.Table[x, y].ToString() + " ");
                    }
                    Console.Out.WriteLine(" ");
                }
                Console.Out.WriteLine("===========================================");
            }
            Console.Out.WriteLine("===========================================");
            Console.Out.WriteLine("NEXT ITER");
            Console.Out.WriteLine("===========================================");****/

            //serialize
            Serializer.Serialize(this,this.ImageSources, this.MainTablesList, this.BestResults, this.ShapesDatabase);
        }

        /// <summary>
        /// Reads width of the Main tables from the file into property value.
        /// </summary>
        /// <param name="p"></param>
        private void SetMainTableWidth(int p)
        {
            TableWidth = p;
        }

        private void ConstructShapes(List<byte[,]> Tiles)
        {
            Random random = new Random();
            //clead list of previoustly loaded shapes
            AllLoadedShapes.Clear();
            foreach (byte[,] tile in Tiles)
            {
                int r = random.Next(100, 220);
                int g = random.Next(100, 220);
                int b = random.Next(100, 220);

                //create and add colors for shapes, add shape to list.
                AllLoadedShapes.Add(new Shape(tile, System.Drawing.Color.FromArgb(0, r, g, b), System.Drawing.Color.FromArgb(0, r + 10, g + 10, b + 10)));
            }

        }

        private void InitializeMainTableList()
        {
            //Create k MAIN tables if they are empty
            if (MainTablesList.Count == 0)
            {
                for (int i = 0; i < k; i++)
                {
                    MainTable mainTable = new MainTable(i);
                    mainTable.Width = this.TableWidth;
                    //In the beginning the height of our tables is equal to the height of the talles Shapes (amount ShapesInfoListWrapper)
                    mainTable.Height = this.MaxShapeHeight;
                    mainTable.Table = new byte[mainTable.Width, mainTable.Height];
                    mainTable.Quantities = (int[])this.ShapeQuantities.Clone();

                    //Add it to the list of a Main Tables
                    this.MainTablesList.Add(mainTable);
                }
            }
        }

        public void ApplyShapes(List<Controls.TileControl> list)
        {
            int shapecount = 0;
            this.RemainingShapes = 0;

            foreach (Controls.TileControl control in list)
            {
                if (control.NumTiles > 0)
                {
                    shapecount++;
                    this.RemainingShapes += control.NumTiles;
                }
            }
            this.ShapesDatabase = new Shape[shapecount];
            this.ShapeQuantities = new int[shapecount];

            for (int i = 0, k = 0; i < list.Count; i++)
            {
                if (list.ElementAt(i).NumTiles > 0)
                {
                    ShapesDatabase[k] = list.ElementAt(i).Shape;
                    ShapeQuantities[k] = list.ElementAt(i).NumTiles;
                    k++;
                }
            }
        }

        internal void StartIteration(int p, int iter)
        {
            this.K = p;
            this.iterLeft = iter;
            this.MaxShapeHeight = this.GetMaxShapeHeight();
            this.ComputationStarted = true;
            InitializeMainTableList();
            threadComp.preformIteration(this, p, MainTablesList);
        }

        /// <summary>
        /// Starting od the computation.
        /// Creates and adds K- MainTables to the list.
        /// Main loops of the algorithm.
        /// </summary>
        /// <param name="k"></param>
        internal void StartComputation(int k)
        {
            this.k = k;
            playing = true;
            this.MaxShapeHeight = this.GetMaxShapeHeight();
            this.ComputationStarted = true;
            InitializeMainTableList();
            //Last argument to getNextIteration is number of iterations to preform
            threadComp.preformIteration(this, k, MainTablesList);
        }

        internal void PauseComputation()
        {
            playing = false;
        }

        private int GetMaxShapeHeight()
        {
            int maxValue = 0;
            for (int i = 0; i < this.ShapesDatabase.Length; i++)
            {
                if (ShapesDatabase[i].MaxHeight > maxValue)
                    maxValue = ShapesDatabase[i].MaxHeight;
            }
            return maxValue;
        }

        internal void ReadListOfImageSources(List<ImageSource> isl)
        {
            this.ImageSources = isl;
        }

        public T[,] ResizeArray<T>(T[,] original, int x, int y)
        {
            T[,] newArray = new T[x, y];
            int minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minY; ++i)
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);
            return newArray;
        }



        public bool SerializeTo(string pathToSerializeInto)
        {
            try
            {
                Serializer.Serialize(this,pathToSerializeInto, this, this.MainTablesList, this.ImageSources/*, this.BestResults, this.ShapesDatabase*/); 
            }
            catch
            {
                return false;
            }

            return true;
	   }

        public bool DeserializeFrom(string dirPath)
        {
           // try
            {
                Serializer.Deserialize(this , dirPath /*,this.ImageSources,  this.MainTablesList, this.BestResults, this.ShapesDatabase*/);
                this.K = this.MainTablesList.Count();
                this.TableWidth = this.MainTablesList.ElementAt(0).Width;
                // tell giu that we have deserialized data
                this.Notify(2); 
            }
           // catch
            {
                //return false;
            }

            return true;
        }


        public void StopComputation()
        {
            this.ComputationStarted = false;
        }




       
    }

}
