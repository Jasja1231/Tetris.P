using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Algorithms;
using Tetris.Windows;

namespace TetrisUnitTests
{
    [TestClass]
    public class ThreadWorkTest
    {
        [TestMethod]
        public void ThreadTest()
        {
            Model m = new Model();
            List<byte[,]> lb = new List<byte[,]>();
            byte[,] bytes = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
            byte[,] bytes2 = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            lb.Add(bytes);
            lb.Add(bytes2);
            m.TableWidth = 10;
            m.ConstructShapes(lb);
            TileBrowser TB = new TileBrowser(m.AllLoadedShapes);
            m.ApplyShapes(TB.TileControls);
            m.StartComputation(1);
            Thread.Sleep(5000);
            m.StopComputation();
            Assert.AreEqual(m.BestResults.Count, 1);
        }
    }
}
