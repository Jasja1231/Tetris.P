using System;
using System.Threading;
using System.Windows;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        public Result work(Model m, MainTable mt, int shapeIdx)
        {
            Result r = new Result(shapeIdx,-1,-1,mt.Kth,-1,-1);
            Shape shape = m.ShapesDatabase[shapeIdx];
            int numberOfShapeTiles = CountTiles(shape);
            double ShapeDensity = numberOfShapeTiles/shape.rotations[0].Length;


            for (int y = 0; y < mt.Table.GetLength(1); y++)
            {
                for(int x = 0; x < mt.Table.GetLength(0); x++)
                {
                    for (int rotation = 0; rotation < shape.rotations.Count; rotation++)
                    {
                        if (x + shape.rotations[rotation].GetLength(0) <= mt.Table.GetLength(0))
                        {
                            var tuple = overlap2(mt.Table, shape.rotations[rotation], x, y);
                            if (!tuple.Item1/*not overlap*/ )
                            {
                                //no overlap
                                //int density =  GetDensity(x, y, shape.rotations[rotation], mt.Table);
                                double shapePositionScore = GetScore(tuple, numberOfShapeTiles, ShapeDensity, y, mt.Table.GetLength(1),
                                    shape.rotations[rotation].GetLength(0) * shape.rotations[rotation].GetLength(1));

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

        
        private int GetDensity(int i, int i1, byte[,] shapeRotation, byte[,] mtTable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a score of shpae placement.
        /// </summary>
        /// <param name="tuple">Tuple<bool,int,int> - is there an overlap, number of tiles of main table in the region,
        /// number of tiles adjacent to the shape's edges</param>
        /// <param name="numberOfShapeTiles"></param>
        /// <param name="shapeDensity"></param>
        /// <returns></returns>
        private double GetScore(Tuple<bool, int, int> tuple, int numberOfShapeTiles, double shapeDensity,int y,int maxTableHeight, int boundingboxsize)
        {
            //Height shape score  1 == GOOD
            double yScore = 1.0 - (y/(double)maxTableHeight);
            
            //Neighbours score 1 == GOOD
            double neighbourScore = (double)tuple.Item3/ (double)numberOfShapeTiles;
            if (neighbourScore > 1) neighbourScore = 1;

           
            //Shape density score  1 == GOOD 
            double boundingBoxDensity = (double)tuple.Item2/(double)boundingboxsize;
            double densityScore = boundingBoxDensity + shapeDensity; /*before + shapedensity after*/;//w zakresie jednego bouding boxa


            return (double)(yScore + neighbourScore + boundingBoxDensity)/3.0;
        }

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
        /// Tuple<bool,int,int> - is there an overlap, number of tiles of main table in the region,
        /// number of tiles adjacent to the shape's edges
        /// </returns>
        public Tuple<bool,int,int> overlap2(byte[,] table, byte[,]shape, int x, int y)
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

        public Result Dummywork(Model m, MainTable mt, int shapeIdx)
        {
            Result r = null;
            int ctr = 0;
            int tresh = 0;
            Point p = new Point();
            CheckDensity cd = new CheckDensity();
            byte[,] tempTable = ResizeArray(mt.Table, mt.Width, (mt.Height + m.ShapesDatabase[shapeIdx].MaxHeight));
            for (int i = tresh; i < tempTable.GetLength(1); i++)
            {
                for (int j = 0; j < tempTable.GetLength(0); j++)
                {
                    if (tempTable[j, i] == 0)
                    {
                        if ((m.ShapesDatabase[shapeIdx].MaxHeight + j) <= (tempTable.GetLength(0)))
                        {
                            for (int y = 0; y < m.ShapesDatabase[shapeIdx].MaxHeight; y++)
                            {
                                for (int x = 0; x < m.ShapesDatabase[shapeIdx].MaxHeight; x++)
                                {
                                    if (tempTable[j + x, i + y] != 0) ctr++;
                                }
                            }
                            /*if (ctr == 0)
                            {
                                p = overlap(tempTable, m.ShapesDatabase[shapeIdx].rotations[bestRotation(shapeIdx, m)], j, i);
                                return new Result(shapeIdx, Convert.ToInt32(p.X), Convert.ToInt32(p.Y), mt.Kth, Convert.ToInt32((100 * cd.checkDensity(mt))), bestRotation(shapeIdx, m));
                            }
                            else
                                ctr = 0;*/
                        }
                    }
                }
                if(i > 2)
                {
                    tresh++; ;
                }
            }
            return r;
        }

        public int bestRotation(int shapeIdx, Model m)
        {
            int rot = 0;
            int longestX = 0, ctr = 0;
            int control = m.ShapesDatabase[shapeIdx].rotations[0].GetLength(0) > m.ShapesDatabase[shapeIdx].rotations[0].GetLength(1) ? 0 : 1;

            for (int rotations_iterator = control; rotations_iterator < (4 - control); rotations_iterator += 2)
            {
                for (int i = 0; i < m.ShapesDatabase[shapeIdx].rotations[rotations_iterator].GetLength(0); i++)
                {
                    if (m.ShapesDatabase[shapeIdx].rotations[rotations_iterator][i, 0] == 1)
                    {
                        ctr++;
                    }
                }
                if (ctr > longestX)
                {
                    longestX = ctr;
                    rot = rotations_iterator;
                    ctr = 0;
                }
            }
            return rot + 2;
        }
        private T[,] ResizeArray<T>(T[,] original, int x, int y)
        {
            var newArray = new T[x, y];
            int minX = Math.Min(x, original.GetLength(0));
            int minY = Math.Min(y, original.GetLength(1));
            for (int i = 0; i < minX; i++)
                for (int j = 0; j < minY; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }


        private Point overlap(byte[,] main, byte[,] shape, int x, int y)
        {
            Point temp = new Point(x, y);
            for (int i = y - shape.GetLength(1); i < y ; i++)
            {
                for (int j = x - shape.GetLength(0); j < x; j++)
                {
                    int ctr = 0;
                    if (j <= main.GetLength(0) && j >= 0 && i >= 0 ) {
                        
                        for (int k = 0; k < shape.GetLength(1); k++)
                        {
                            for (int l = 0; l < shape.GetLength(0); l++)
                            {
                                if (Convert.ToInt32(shape[l, k]) + Convert.ToInt32(main[j + l, i + k]) == 2)
                                {
                                    ctr++;
                                }
                            }
                        }
                        if (ctr == 0 && (temp.Y < y))
                        {
                            y = Convert.ToInt32(temp.Y);
                            if(temp.X < x)
                            {
                                x = Convert.ToInt32(temp.X);
                            }
                        }
                    }
                }
            }
            return new Point(x,y) ;
        }

        private int cntArray(byte[,] shape)
        {
            int ct = 0;
            foreach (byte b in shape)
            {
                ct += Convert.ToInt32(b);
            }
            return ct;
        }
    }
}