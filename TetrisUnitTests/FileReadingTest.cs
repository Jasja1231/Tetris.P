using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris.Algorithms;
using System.Linq;

namespace TetrisUnitTests
{
    [TestClass]
    public class FileReadingTest
    {
        [TestMethod]
        public void FileReading()
        {
            string content = "10\n2 2\n0 0\n0 0\n2 2\n1 1\n1 1";
            var split = content.Split(Environment.NewLine.ToCharArray());
            var res = FileReader.GetBricksFromFile(split);
            Assert.AreEqual(10, res.Item2);
            for (int i=0;i<2;i++)
            {
                for(int j=0;j<2;j++)
                Assert.AreEqual(0,res.Item1[0][i,j]);
            }
        }
    }
}
