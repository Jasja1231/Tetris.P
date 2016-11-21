using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    class CheckDensity
    {
        public double checkDensity(byte[,] Table)
        {
            int ctr = 0;
            foreach(byte bt in Table) {
                ctr += Convert.ToInt32(bt);
                }
            return ctr/(Table.GetLength(1) * Table.GetLength(0));
        }
        public double Evaluate(MainTable Table)
        {
            return 0;
        }

    }
}

