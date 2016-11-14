using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    class MainTable
    {
        //*********************************CLASS FIELDS****************************************/
        public byte[,] table { get; private set; }
        public int Kth { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int maxHeight { get; private set; }
        //*********************************CLASS METHODS***************************************/
        public MainTable(int Kth)
        {
            this.Kth = Kth;
        }
    }
}
