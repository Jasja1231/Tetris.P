using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Algorithms;

namespace TetrisUnitTests
{
    [TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            byte[,]disconnected = new byte[4,4]{{0,0,1,0},{0,1,0,0},{0,0,1,0},{0,0,1,0}};
            byte[,]hole =  new byte[4,4]{{1,1,1,1},{1,0,0,1},{1,0,0,1},{1,1,1,1}};
            byte[,]valid =  new byte[4,4]{{0,0,1,0},{0,0,1,0},{0,0,1,0},{0,0,1,0}};
            Shape Disconnected = new Shape(disconnected, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
            Shape Hole = new Shape(hole, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
            Shape Valid = new Shape(valid, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));

            Assert.AreEqual(0, ShapeValidator.isTileValid(Valid));
            Assert.AreEqual(1, ShapeValidator.isTileValid(Hole));
            Assert.AreEqual(2, ShapeValidator.isTileValid(Disconnected));
        }
    }
}
