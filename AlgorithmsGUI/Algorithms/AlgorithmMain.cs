using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
    public class AlgorithmMain : Tetris.ObserverDP.Subject
    {
        public AlgorithmMain() { 
        
        }
        /// <summary>
        /// List of tiles (shapes) 
        /// </summary>
        private List<Shape> shapes = new List<Shape>();
        public List<Shape> Shapes
        {
            get { return shapes; }
            private set { shapes = value; }
        }
        /// <summary>
        /// Adds a tile to a list of shapes
        /// </summary>
        /// <param name="s">Shape to be added</param>
        public void AddShapeToList(Shape s){
            this.shapes.Add(s);
        }
        /// <summary>
        /// Backtracking parameter. The user set K variable, constant for now
        /// </summary>
        private int k = 5;
        public int K
        {
            get { return k; }
            private set { k = value; }
        }



    }
}
