using System.Drawing;
using Tetris.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TetrisUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ShapeComparisonTest()
        {
            byte[,] bytes = { {0,0,0}, {1,1,1}, {0,0,0} };
            Shape_comparer sc = new Shape_comparer();
            Shape s1 = new Shape(bytes, Color.AliceBlue, Color.AliceBlue);
            Shape s2 = new Shape(bytes, Color.Aqua, Color.Aqua);
            Assert.Equals(true, sc.Equals(s1, s2));
        }
    }
}
