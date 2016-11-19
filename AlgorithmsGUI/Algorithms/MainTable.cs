using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    public class MainTable
    {
        //*********************************CLASS FIELDS****************************************/
        public byte[,] Table { get;  set; }
        /// <summary>
        /// Index K of Main Table.
        /// </summary>
        public int[] Quantities;
        public int Kth { get;  set; }
        /// <summary>
        /// Width of the table
        /// </summary>
        public int Width { get;  set; }
        /// <summary>
        /// Height of the main table
        /// </summary>
        public int Height { get;  set; }
        /// <summary>
        /// ???
        /// </summary>
        public int MaxHeight { get;  set; }

        /// <summary>
        /// Each Main table with shape indexs (from some common tables, f.e. hashMap) not to keep copies 
        /// of Shape tables, and then avoid deep copying list of Shapes where we can only copy integers.
        /// TODO: Shared table of shapes is to be created in Model.
        /// </summary>
        public List<int> indexesOfUsedShapes { get; set;}


        //*********************************CLASS METHODS***************************************/
        public MainTable(int Kth/*,width,height*/)
        {
            this.Kth = Kth;
            //Create table [][] in here
            //table = new byte[width][height];
        }
    }
}
