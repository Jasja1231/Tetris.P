using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class ShapeValidator
    {
        /// <summary>
        /// Checks if shape (tile) is valid, meaning that shape doesn't have any holes and is not disconnected.
        /// </summary>
        /// <param name="shape"> shape to be checked for validity</param>
        /// <returns>int representing wheter the tile is valid or not in the fillowing way : 
        ///  0-tile is valid
        ///  1-tile is hollow
        ///  2-tile is disconnected
        /// </returns>
        public int isShapeValid(Shape shape)
        {
            return 0;
        }

    }
}
