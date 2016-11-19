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
            Result r = new Result(shapeIdx, rnd.Next(0,100), rnd.Next(0, 100), mt.Kth, rnd.Next(101));
            return r;
        }
        public Result work(Model m, MainTable mt, int shapeIdx)
        {
            Result r = null;
            int ctr = 0;
            for (int i = 0; i < mt.Height; i++)
            {
                for (int j = 0; j < mt.Width; j++)
                {
                    if (mt.Table[i, j] == 0)
                    {
                        for(int y = 0; y < m.ShapesDatabase[shapeIdx].MaxHeight; y++)
                        {
                            for(int x = 0; x < m.ShapesDatabase[shapeIdx].MaxHeight; x++)
                            {
                                if (mt.Table[i + y, j + x] != 0) ctr++;
                            }
                        }
                        if (ctr == 0) 
                            return new Result(shapeIdx, j, i, mt.Kth, 13);
                        else 
                            ctr = 0;
                    }
                }
            }
            return r;
        }
    }
}
