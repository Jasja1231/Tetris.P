using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Algorithms;
using System.Collections.Generic;

namespace TetrisUnitTests
{
    [TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void ValidationTest()
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

       [TestMethod]
        public void EqualityTest()
        {
            byte[,] arr = new byte[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } };
            byte[,] arr2 = new byte[4, 4] { { 1, 1, 1, 1 }, { 1, 0, 0, 1 }, { 1, 0, 0, 1 }, { 1, 1, 1, 1 } };
            Shape s1 = new Shape(arr, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
            Shape otherS1 = new Shape(arr, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
            Shape other = new Shape(arr2, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));

            Assert.IsTrue(ShapeValidator.AreEqual(s1, otherS1));
            Assert.IsFalse(ShapeValidator.AreEqual(s1, other));
        }

        [TestMethod]
        public void DuplicateTest()
       {
           var Shapes = new List<Tetris.Controls.TileControl>();
           byte[,] arr = new byte[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } };
           byte[,] arr2 = new byte[4, 4] { { 1, 1, 1, 1 }, { 1, 0, 0, 1 }, { 1, 0, 0, 1 }, { 1, 1, 1, 1 } };
           Shape s1 = new Shape(arr, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
           Shape otherS1 = new Shape(arr, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
           Shape other = new Shape(arr2, System.Drawing.Color.FromArgb(0, 0, 0, 0), System.Drawing.Color.FromArgb(0, 0, 0, 0));
           var tc1 = new Tetris.Controls.TileControl(s1);
           var tc2 = new Tetris.Controls.TileControl (otherS1);
           var tc3 = new Tetris.Controls.TileControl (other);
           Shapes.Add(tc1);
           Shapes.Add(tc2);
           Shapes.Add(tc3);
           ShapeValidator.MarkDuplicates(Shapes);
           Assert.IsFalse(Shapes[0].IsDuplicate);
           Assert.IsTrue(Shapes[1].IsDuplicate);
           Assert.IsFalse(Shapes[2].IsDuplicate);
       }
    }
}
