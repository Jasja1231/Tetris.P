using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    class CheckDensity
    {
        public double checkDensity(MainTable Table)
        {
            int ctr = 0;
            foreach(byte bt in Table.Table) {
                ctr += Convert.ToInt32(bt);
                }
            return ctr/(Table.Height * Table.Width);
        }
        public double Evaluate(MainTable Table)
        {
            return 0;
        }

    }
}

