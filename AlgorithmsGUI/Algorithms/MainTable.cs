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
        /// <summary>
        /// Index K of Main Table.
        /// </summary>
        public int Kth { get; private set; }
        /// <summary>
        /// Width of the table
        /// </summary>
        public int width { get; private set; }
        /// <summary>
        /// Height of the main table
        /// </summary>
        public int height { get; private set; }
        /// <summary>
        /// ???
        /// </summary>
        public int maxHeight { get; private set; }
        //*********************************CLASS METHODS***************************************/
        public MainTable(int Kth/*,width,height*/)
        {
            this.Kth = Kth;
            //Create table [][] in here
            //table = new byte[width][height];
        }
    }
}
