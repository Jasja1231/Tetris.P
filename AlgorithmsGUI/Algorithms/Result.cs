using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    /// <summary>
    /// Class used by thread  to return result of its' computation.
    /// </summary>
    public class Result
    {
        //*********************************CLASS FIELDS****************************************/
        /// <summary>
        /// Shape that we are adding to Main Table
        /// </summary>
        public Shape s { get; private set; }
        /// <summary>
        /// X coordinate of Kth table where we are putting shape s
        /// </summary>
        public int x { get; private set; }
        /// <summary>
        /// Y coordinate of Kth table where we are putting shape s
        /// </summary>
        public int y { get; private set; }
        /// <summary>
        /// Index of main table we are adding our shape into 
        /// </summary>
        public int Kth { get; private set; }
        /// <summary>
        /// Estimated 'how good' our shape is positiones on Kth main table
        /// </summary>
        public int score { get; private set; }

        public int rotation { get;  set; }
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
