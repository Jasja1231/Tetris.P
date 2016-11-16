using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        public Result work(MainTable mt, Shape s)
        {
            //TODO: remove stub work, implement real density check and evalutation
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int sleeptime = rnd.Next(10000, 30000);
            Thread.Sleep(sleeptime);
            Result r = new Result(s, rnd.Next(0,100), rnd.Next(0, 100), mt.Kth, rnd.Next(101));
            return r;
        }
    }
}
