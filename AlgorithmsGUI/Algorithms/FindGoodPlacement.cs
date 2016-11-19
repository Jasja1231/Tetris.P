using System;
using System.Threading;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        public Result dwork(MainTable mt, Shape s)
        {
            //TODO: remove stub work, implement real density check and evalutation
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int sleeptime = rnd.Next(5000, 10000);
            Thread.Sleep(sleeptime);
            Result r = new Result(s, rnd.Next(0,100), rnd.Next(0, 100), mt.Kth, rnd.Next(101));
            return r;
        }
        public Result work(MainTable Table, Shape s)
        {
            Result r = null;
            int ctr = 0;
            for (int i = 0; i < Table.Height; i++)
            {
                for (int j = 0; j < Table.Width; j++)
                {
                    if (Table.Table[i, j] == 0)
                    {
                        for(int y = 0; y < s.MaxHeight; y++)
                        {
                            for(int x = 0; x < s.MaxHeight; x++)
                            {
                                if (Table.Table[i + y, j + x] != 0) ctr++;
                            }
                        }
                        if (ctr == 0) return r = new Result(s, j, i, Table.Kth, 13);
                        else ctr = 0;
                    }
                }
            }
            return r;
        }
    }
}
