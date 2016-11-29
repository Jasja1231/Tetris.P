using System;
using Tetris.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tetris.Windows;

namespace TetrisUnitTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void WeightsChangedTest()
        {
            byte[,] bytes = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
            Model m = new Model();
            m.UpdateWeights(10, 20, 10);
            Assert.AreEqual(m.YPositionWeight, 10);
            Assert.AreEqual(m.BoxDensityWeight, 20);
            Assert.AreEqual(m.NeighborWeight, 10);
            Assert.AreEqual(m.WeightDivisor, 40);
        }
        [TestMethod]
        public void CostructShapesTest()
        {
            Model m = new Model();
            List<byte[,]> lb = new List<byte[,]>();
            byte[,] bytes = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
            byte[,] bytes2 = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            lb.Add(bytes);
            lb.Add(bytes2);
            m.TableWidth = 10;
            m.ConstructShapes(lb);
            Assert.AreEqual(m.AllLoadedShapes.Count, 2);
        }
        [TestMethod]
        public void ApplyShapesTest()
        {
            Model m = new Model();
            List<byte[,]> lb = new List<byte[,]>();
            byte[,] bytes = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
            byte[,] bytes2 = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            lb.Add(bytes);
            lb.Add(bytes2);
            m.TableWidth = 10;
            m.ConstructShapes(lb);
            Assert.IsNull(m.ShapesDatabase);
            Assert.IsNull(m.ShapeQuantities);
            TileBrowser TB = new TileBrowser(m.AllLoadedShapes);
            m.ApplyShapes(TB.TileControls);
            Assert.AreEqual(m.ShapesDatabase.Length, 1);
            Assert.AreEqual(m.ShapeQuantities.Length, 1);
        }
        [TestMethod]
        public void StartComputationTest()
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
            m.StopComputation();
            Assert.AreNotEqual(m.StopWatch, new System.Diagnostics.Stopwatch());
        }
        [TestMethod]
        public void PauseComputationTest()
        {
            Model m = new Model();
            m.StopWatch.Start();
            m.playing = true;
            m.PauseComputation();
            Assert.IsFalse(m.playing);
            Assert.AreNotEqual(m.StopWatch, new System.Diagnostics.Stopwatch());

        }
        [TestMethod]
        public void StopComputationTest()
        {
            Model m = new Model();
            m.StopWatch.Start();
            m.ComputationStarted = true;
            m.StopComputation();
            Assert.IsFalse(m.ComputationStarted);
            Assert.AreNotEqual(m.StopWatch, new System.Diagnostics.Stopwatch());

        }
    }
}
