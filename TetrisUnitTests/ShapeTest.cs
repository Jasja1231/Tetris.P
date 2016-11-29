using System.Drawing;
using Tetris.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris;

namespace TetrisUnitTests
{
    [TestClass]
    public class ShapeTest
    {
        [TestMethod]
        public void ShapeComparisonTest()
        {
            byte[,] bytes = { {0,0,0}, {1,1,1}, {0,0,0} };
            byte[,] bytes2 = { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 } };
            Shape_comparer sc = new Shape_comparer();
            Shape s1 = new Shape(bytes, Color.AliceBlue, Color.AliceBlue);
            Shape s2 = new Shape(bytes, Color.Aqua, Color.Aqua);
            Shape s3 = new Shape(bytes2, Color.Black,Color.Black);

            Assert.AreEqual(true, sc.Equals((Shape)s1, (Shape)s1));
            Assert.AreEqual(true, sc.Equals((Shape)s1, (Shape)s2));
            Assert.AreEqual(false, sc.Equals((Shape)s1, (Shape)s3));
        }
    }
}
