using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tetris.Algorithms
{
    class FindGoodPlacement
    {
        public int work()
        {
            //STUB WORK
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int sleeptime = rnd.Next(10000, 30000);
            Thread.Sleep(sleeptime);
            return sleeptime;
        }
    }
}
