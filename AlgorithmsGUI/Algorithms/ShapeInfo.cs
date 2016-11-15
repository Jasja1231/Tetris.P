using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    /// <summary>
    /// Pair of Shape and its count
    /// </summary>
    class ShapeInfo
    {

        private Shape shape;
        private int availabletiles;
        public Shape Shape
        {
            get { return shape; }
            private set { shape = value; }
        }
        public int AvailableTiles
        {
            get { return availabletiles; }
            private set { availabletiles = value; }
        }

        public ShapeInfo (Shape sh, int available)
        {
            Shape = sh;
            AvailableTiles = available;
        }

        public bool AreTilesAvailable ()
        {
            return AvailableTiles>0?true:false;
        }

        public void RemoveTile()
        {
            if (AreTilesAvailable())
            {
                AvailableTiles--;
            }
            else
                throw new InvalidOperationException("No Tiles available");
        }
    }
}
