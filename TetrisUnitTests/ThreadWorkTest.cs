using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Algorithms;

namespace TetrisUnitTests
{
    [TestClass]
    public class ThreadWorkTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Model m = new Model();
            m.BestResults = new List<Result>();
            m.ShapesDatabase = new Shape[1];
            byte[,] bytes = {{0, 1}, {0, 1}};
            m.ShapesDatabase[0] = new Shape(bytes, Color.Black, Color.Black);
            ThreadComputation tc = new ThreadComputation(m);
            List<MainTable> lmt = new List<MainTable>();
            MainTable mainTable = new MainTable(0);
            mainTable.Width = 3;
            //In the beginning the height of our tables is equal to the height of the talles Shapes (amount ShapesInfoListWrapper)
            mainTable.Height = 3;
            mainTable.Table = new byte[mainTable.Width, mainTable.Height];
            mainTable.Quantities = new int[] { 1 };
            //Add it to the list of a Main Tables
            lmt.Add(mainTable);
            tc.preformIteration(m,1,lmt);
            Thread.Sleep(5000);
            Assert.Equals(m.BestResults.Count, 1);
        }
    }
}
