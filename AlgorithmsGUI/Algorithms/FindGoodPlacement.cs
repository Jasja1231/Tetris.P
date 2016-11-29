using System;
using System.Threading;
using System.Windows;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        //Thread work algorithm
        public Result work(Model m, MainTable mt, int shapeIdx)
        {
            Result r = new Result(shapeIdx,-1,-1,mt.Kth,-1,-1);
            Shape shape = m.ShapesDatabase[shapeIdx];
            int numberOfShapeTiles = CountTiles(shape);
            double ShapeDensity = numberOfShapeTiles/shape.rotations[0].Length;

            //<= because we want to iterate over height of table which doesn't exits yet!
            for (int y = 0; y <= mt.Table.GetLength(1); y++)
            {
                for(int x = 0; x < mt.Table.GetLength(0); x++)
                {
                    for (int rotation = 0; rotation < shape.rotations.Count; rotation++)
                    {
                        if (x + shape.rotations[rotation].GetLength(0) <= mt.Table.GetLength(0))
                        {
                            var tuple = Overlap(mt.Table, shape.rotations[rotation], x, y);
                            if (!tuple.Item1/*not Overlap*/ )
                            {
                                //no Overlap
                                //int density =  GetDensity(x, y, shape.rotations[rotation], mt.Table);
                                double shapePositionScore = GetScore(tuple, numberOfShapeTiles, ShapeDensity, y, mt.Table.GetLength(1),
                                    shape.rotations[rotation].GetLength(0) * shape.rotations[rotation].GetLength(1),m.YPositionWeight,m.NeighborWeight,m.BoxDensityWeight,m.WeightDivisor);

                                int newScore = (int)(shapePositionScore * 100); // GetOveralScore();

                                if (newScore > r.score)
                                {
                                    r.score = newScore;
                                    r.x = x;
                                    r.y = y;
                                    r.rotation = rotation;
                                }
                            }
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// Returns a score of shpae placement.
        /// </summary>
        /// <param name="tuple">Tuple<bool,int,int> - is there an Overlap, number of tiles of main table in the region,
        /// number of tiles adjacent to the shape's edges</param>
        /// <param name="numberOfShapeTiles"></param>
        /// <param name="shapeDensity"></param>
        /// <returns></returns>
        private double GetScore(Tuple<bool, int, int> tuple, int numberOfShapeTiles, double shapeDensity,int y,int maxTableHeight, int boundingboxsize, int yweight, int neighweight, int boxweight, int divisor)
        {
            //Height shape score  1 == GOOD
            double yScore = 1.0 - (y/(double)maxTableHeight);
            
            //Neighbours score 1 == GOOD
            double neighbourScore = (double)tuple.Item3/ (double)numberOfShapeTiles;
            if (neighbourScore > 1) neighbourScore = 1;

           
            //Shape density score  1 == GOOD 
            double boundingBoxDensity = (double)tuple.Item2/(double)boundingboxsize;
            double densityScore = boundingBoxDensity + shapeDensity; /*before + shapedensity after*/;//w zakresie jednego bouding boxa


            return (double)(yweight*yScore + neighweight*neighbourScore + boxweight*boundingBoxDensity)/divisor;
        }
        //count tiles in shape's bounding box
        private int CountTiles(Shape shape)
        {
            int count = 0;
            for (int i = 0; i < shape.rotations[0].GetLength(0); i++)
            {
                for (int j = 0; j < shape.rotations[0].GetLength(1); j++)
                {
                    if (shape.rotations[0][i, j] == 1)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="shape"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>
        /// Tuple<bool,int,int> - is there an Overlap, number of tiles of main table in the region,
        /// number of tiles adjacent to the shape's edges
        /// </returns>
        private Tuple<bool,int,int> Overlap(byte[,] table, byte[,]shape, int x, int y)
        {
            //i = iterator for main table y's
            //j = iterator for main table x's
            //i2 = iterator for shape y's
            //j2 = iterator for shape x's
            int adjacentCount = 0;
            int mainTableTiles = 0;// bysy cells around our tile 
            for (int i = y, i2 = shape.GetLength(1)-1; i2 >= 0; i++, i2--)
            {
                for (int j = x, j2 = 0; j2 < shape.GetLength(0); j++, j2++)
                {
                    //if y is not out of main table's range
                    if (i < table.GetLength(1))
                    {
                        if (table[j, i] == (byte) 1 && shape[j2, i2] == (byte) 1)
                            return new Tuple<bool, int, int>(true,-1,-1);
                        else if (table[j, i] == (byte)1)
                            mainTableTiles++;

                        if (shape[j2, i2] == 1) //for all shape's 1's we count adjacent neighbor 1's if they exist
                        {
                            adjacentCount += SafeNeighborCount(table, j, i);
                        }
                    }
                    else
                    {
                        // went out or range into empty part of table
                        if (shape[j2, i2] == 1) //for all shape's 1's we count adjacent neighbor 1's if they exist
                        {
                            adjacentCount += SafeNeighborCount(table, j, i);
                        }
                    }

                }
            }
           return new Tuple<bool, int, int>(false,mainTableTiles, adjacentCount);
        }
        //check if shape at [j,i] has neigbors in mainTable's byteArray
        private int SafeNeighborCount(byte[,] table, int j , int i )
        {
            int adjacentCount = 0;
            //up
            if (i+1 < table.GetLength(1) && j >= 0 && j < table.GetLength(0) &&  table[j, i + 1] == (byte)1)
                adjacentCount++;
            //down
            if (i-1 >= 0 && j >= 0 && j < table.GetLength(0) && i-1 < table.GetLength(1) && table[j, i - 1] == (byte)1)
                adjacentCount++;
            //left
            if (j-1 >=0 && i >= 0 && i < table.GetLength(1) && table[j - 1, i] == (byte)1)
                adjacentCount++;
            //right
            if (j+1 < table.GetLength(0) && i >= 0 && i < table.GetLength(1) && table[j + 1, i] == (byte)1)
                adjacentCount++;
          
            return adjacentCount;
        }
    }
}