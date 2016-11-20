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
        private MainTable mainTable;

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

        //*********************************CLASS METHODS***************************************/
        public MainTable(int Kth/*,width,height*/)
        {
            this.Kth = Kth;
            //Create table [][] in here
            //table = new byte[width][height];
        }

        internal MainTable UpdateWithResult(Result r, Model m)
        {
            MainTable mt = new MainTable(r.Kth);
            mt.Quantities = new int[this.Quantities.Length];
            mt.Width = this.Width;
            int newHeight = r.y + m.ShapesDatabase[r.shapeIdx].rotations.ElementAt(r.rotation).GetLength(1);
            if (newHeight > this.Height)
            {
                //create bigger array
                mt.Table = m.ResizeArray(this.Table, Width, newHeight);
                mt.Height = newHeight;
            }
            else
            {
                mt.Table = m.ResizeArray(this.Table, Width, Height);
                mt.Height = this.Height;
            }
            //decrease used shape quantity
            Array.Copy(this.Quantities, mt.Quantities, this.Quantities.Length);
            mt.Quantities[r.shapeIdx]--;
            this.AddShapeToTable(mt, m.ShapesDatabase[r.shapeIdx], r);
            return mt;
        }

        private void AddShapeToTable(MainTable mt, Shape s, Result r)
        {
            byte[,] table = s.rotations.ElementAt(r.rotation);
            //i = iterator for main table y's
            //j = iterator for main table x's
            //i2 = iterator for shape y's
            //j2 = iterator for shape x's
            for (int i = r.y + table.GetLength(1)-1, i2 = 0; i2 < table.GetLength(1); i-- , i2++)
            {
                for (int j = r.x, j2 = 0; j2 < table.GetLength(0); j++, j2++)
                {
                    mt.Table[j, i] = table[j2, i2] == 1 ?  (byte)1: (byte)0;
                }
            }
        }
    }
}