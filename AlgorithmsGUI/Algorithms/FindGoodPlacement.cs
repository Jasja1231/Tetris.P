﻿using System;
using System.Threading;
using System.Windows;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        public Result dwork(Model m, MainTable mt, int shapeIdx)
        {
            //TODO: remove stub work, implement real density check and evalutation
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int sleeptime = rnd.Next(5000, 10000);
            Thread.Sleep(sleeptime);
            Result r = new Result(shapeIdx, rnd.Next(0, 100), rnd.Next(0, 100), mt.Kth, rnd.Next(101), 2);
            return r;
        }
        public Result work(Model m, MainTable mt, int shapeIdx)
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
                            if (ctr == 0)
                            {
                                p = overlap(tempTable, m.ShapesDatabase[shapeIdx].rotations[bestRotation(shapeIdx, m)], j, i);
                                return new Result(shapeIdx, Convert.ToInt32(p.X), Convert.ToInt32(p.Y), mt.Kth, Convert.ToInt32((100 * cd.checkDensity(mt))), bestRotation(shapeIdx, m));
                            }
                            else
                                ctr = 0;
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
        private T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
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