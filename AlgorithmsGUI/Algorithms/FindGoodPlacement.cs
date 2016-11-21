using System;
using System.Threading;
using System.Windows;

namespace Tetris.Algorithms
{
	class FindGoodPlacement
	{
		public Result dwork(Model m, MainTable mt, int shapeIdx)
		{
			//TODO: remove stub work, implement real density check and evalutation
			Random rnd = new Random(Guid.NewGuid().GetHashCode());
			int sleeptime = rnd.Next(5000, 10000);
			Thread.Sleep(sleeptime);
			Result r = new Result(shapeIdx, rnd.Next(0, 100), rnd.Next(0, 100), mt.Kth, rnd.Next(101), 2);
			return r;
		}
		public Result work(Model m, MainTable mt, int shapeIdx)
		{
			CheckDensity cd = new CheckDensity();
			byte[,] tempTable = ResizeArray(mt.Table, mt.Width, mt.Height);
            while (true)
            {
            for (int rot = 0; rot < 4; rot++)
                {
                    for (int i = 0; i < tempTable.GetLength(1); i++)
                    {
                        if(i == tempTable.GetLength(1)- m.ShapesDatabase[shapeIdx].rotations[rot].GetLength(1))
                        {
                            tempTable = ResizeArray(mt.Table, mt.Width, (mt.Height + m.ShapesDatabase[shapeIdx].rotations[bestRotation(shapeIdx, m)].GetLength(1)));
                        }
                        for (int j = 0; j < tempTable.GetLength(0) - m.ShapesDatabase[shapeIdx].rotations[rot].GetLength(0); j++)
                        {
                            if (!overlap(tempTable, m.ShapesDatabase[shapeIdx].rotations[rot], j, i))
                            {
                                for (int y = 0; y < (m.ShapesDatabase[shapeIdx].rotations[rot].GetLength(1)); y++)
                                {
                                    for (int x = 0; x < (m.ShapesDatabase[shapeIdx].rotations[rot].GetLength(0)); x++)
                                    {
                                        tempTable[j + x, i + y] = m.ShapesDatabase[shapeIdx].rotations[rot][x, y];
                                    }
                                }
                                return new Result(shapeIdx, j, i, mt.Kth, Convert.ToInt32((100 * cd.checkDensity(tempTable))), rot);
                            }
                        }
                    }
                }
                tempTable = ResizeArray(mt.Table, mt.Width, (mt.Height + m.ShapesDatabase[shapeIdx].rotations[bestRotation(shapeIdx, m)].GetLength(1)));
            }
		}
		public int bestRotation(int shapeIdx, Model m)
		{
			int rot = 0;
			int longestX = 0, ctr = 0;
			int control = m.ShapesDatabase[shapeIdx].rotations[0].GetLength(0) > m.ShapesDatabase[shapeIdx].rotations[0].GetLength(1) ? 0 : 1;

			for (int rotations_iterator = control; rotations_iterator < (4 - control); rotations_iterator += 2)
			{
				for (int i = 0; i < m.ShapesDatabase[shapeIdx].rotations[rotations_iterator].GetLength(0); i++)
				{
					if (m.ShapesDatabase[shapeIdx].rotations[rotations_iterator][i, 0] == 1)
					{
						ctr++;
					}
				}
				if (ctr > longestX)
				{
					longestX = ctr;
					rot = rotations_iterator;
					ctr = 0;
				}
			}
			return rot + 2;
		}
		private T[,] ResizeArray<T>(T[,] original, int rows, int cols)
		{
			var newArray = new T[rows, cols];
			int minRows = Math.Min(rows, original.GetLength(0));
			int minCols = Math.Min(cols, original.GetLength(1));
			for (int i = 0; i < minRows; i++)
				for (int j = 0; j < minCols; j++)
					newArray[i, j] = original[i, j];
			return newArray;
		}
		private bool overlap(byte[,] main, byte[,] shape, int x, int y)
		{
			int ctr = 0;
			if ((x+shape.GetLength(0)) <= main.GetLength(0) && (y + shape.GetLength(1)) <= main.GetLength(1)) {	
				for (int k = 0; k < shape.GetLength(1); k++)
				{
					for (int l = 0; l < shape.GetLength(0); l++)
					{
						if (shape[l, k] + main[x + l, y + k] > 1)
						{
							ctr++;
						}
					}
				}
				if (ctr == 0)
				{
					return false;
				}	
			}
			return true;
		}

		private int cntArray(byte[,] shape)
		{
			int ct = 0;
			foreach (byte b in shape)
			{
				ct += Convert.ToInt32(b);
			}
			return ct;
		}
	}
}