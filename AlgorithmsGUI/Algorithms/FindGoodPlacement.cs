using System;
using System.Threading;

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
            int ctr = 0, control = 0;

            for (int i = 0; i < mt.Height; i++)
            {
                for (int j = 0; j < mt.Width; j++)
                {
                    if (mt.Table[j, i] == 0)
                    {

                        for (int y = 0; y < m.ShapesDatabase[shapeIdx].MaxHeight; y++)
                        {
                            for (int x = 0; x < m.ShapesDatabase[shapeIdx].MaxHeight; x++)
                            {
                                if (mt.Table[j + x, i + y] != 0) ctr++;
                            }
                        }
                        if (ctr == 0)
                            return new Result(shapeIdx, j, i, mt.Kth, 13, bestRotation(shapeIdx, m));
                        else
                            ctr = 0;
                    }
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
    }
}