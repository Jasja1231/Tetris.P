using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    /// <summary>
    /// Class that stores list of ShapeInfo (Shape and its count)
    /// </summary>
    public class ShapesInfoListWrapper

    {
        private List<ShapeInfo> availableshapes;
        public int RemainingTiles { get; set; }

        public List<ShapeInfo> AvailableShapes
        {
            get
            {
                return availableshapes;
            }
            private set 
            {
                if (value !=null)
                    availableshapes = value; 
            }
        }
        /// <summary>
        /// Fills AvailableShapes with ShapesInfoListWrapper and their amounts taken from TileControls
        /// </summary>
        /// <param name="ControlList"></param>
        public void BuildList (List<Controls.TileControl>ControlList)
        {
            //clear list to have it reasigned.
            this.AvailableShapes.Clear();

            foreach (Controls.TileControl tc in ControlList)
            {
               // if (tc.NumTiles > 0 && !(ShapeValidator.isTileValid(tc.Shape) > 0))
                if (tc.NumTiles > 0) //NumTiles is set to 0 for invalid tiles by the control
                {
                    AvailableShapes.Add(new ShapeInfo(tc.Shape, tc.NumTiles));
                    RemainingTiles += tc.NumTiles;
                }
            }
        }


        public Shape GetShapeAt (int index)
        {
            return AvailableShapes.ElementAt(index).Shape;
        }

        /// <summary>
        /// Decrements the number of shapes at given index, removes it from the list if it is empty
        /// </summary>
        /// <param name="index"></param>
        /// <returns>
        /// true on success, 0 if tile could not be removed
        /// </returns>
        public bool RemoveTileAt(int index)
        {
            try 
            {
                AvailableShapes.ElementAt(index).RemoveTile();
                if (AvailableShapes.ElementAt(index).AvailableTiles == 0)
                    AvailableShapes.RemoveAt(index);
                RemainingTiles--;
                return true;
            }
            catch (InvalidOperationException) //No more tiles at this index are available
            {
                return false;
            }
        }


        public ShapesInfoListWrapper ()
        {
            AvailableShapes = new List<ShapeInfo>();
        }
    }
}
