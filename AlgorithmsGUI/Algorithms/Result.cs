using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    class Result
    {
        //*********************************CLASS FIELDS****************************************/
        public Shape s { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public int Kth { get; private set; }
        public int score { get; private set; }
        //*********************************CLASS METHODS***************************************/
        public Result(Shape s, int x, int y, int Kth, int score)
        {
            this.s = s;
            this.x = x;
            this.y = y;
            this.Kth = Kth;
            this.score = score;
        }
    }
}
