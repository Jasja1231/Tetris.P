using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Algorithms
{
    class ThreadComputation
    {
        private static volatile Boolean work;

        public static void getNextIteration(int K, List<MainTable> lmt, ShapesInfoListWrapper sil, int iter)
        {
            //THIS WORKER THREAD SHOULD HAVE A WAY TO UPDATE GUI SO IT CAN UPDATE IT WITHOUT BLOCKING OUR ENTIRE APPLICATION
            //OR
            //WE CAN BLOCK HERE USING worker.join(), AND WAIT FOR THIS THREAD TO GIVE US BACK RESULTS
            //OR
            //MAYBE THERE EXISTS SOME OTHER C# WAY I DONT KNOW ABOUT?
            List<Result> results = new List<Result>();
            work = true;
            Thread worker = new Thread(() => { results = preformIteration(K, lmt, sil, iter); });
            worker.Start();
        }

        public static void pauseComputation()
        {
            work = false;
        }

        private static List<Result> preformIteration(int K, List<MainTable> lmt, ShapesInfoListWrapper sil, int iter)
        {
            int iteration = 0;
            while (work && iteration < iter)
            {
                //start tasks with (LongRunning) work
                int numOfTasks = lmt.Count * sil.AvailableShapes.Count;
                Task<Result>[] tasks = new Task<Result>[numOfTasks];
                List<Result> bestResults = new List<Result>(K);
                FindGoodPlacement fpg = new FindGoodPlacement();
                string display = "Best K results\n";

                //for each MAIN TABLE == from 0 until K
                for (int i = 0; i < lmt.Count; i++)
                {
                    MainTable mt = lmt.ElementAt(i);
                    //for each shape
                    for (int j = 0; j < sil.AvailableShapes.Count; j++)
                    {
                        Shape temp = sil.GetShapeAt(i);
                        //CREATE THREAD to find its position and start its THREAD_WORK
                        tasks[sil.AvailableShapes.Count * i + j] = Task<Result>.Factory.StartNew(() =>
                        {
                            return fpg.work(mt, temp);
                        }, TaskCreationOptions.LongRunning);
                    }
                }
                //BLOCK UNTILL ALL THREADS FINNISH
                Task.WaitAll(tasks);
                //Copy K best results into our list of best results(MAIN TABLES?)
                bestResults = SelectionSort(tasks, K);
                for (int i = 0; i < K; i++)
                {
                    display += "MainTable=" + bestResults.ElementAt(i).Kth + ":(" + bestResults.ElementAt(i).x + "," +
                       bestResults.ElementAt(i).y + "), score=" + bestResults.ElementAt(i).score + "\n";
                }
                MessageBox.Show(display);
                iter++;
            }
            return null;
        }

        //Complexity O(n) not O(n^2) since I only find K largest :)
        private static List<Result> SelectionSort(Task<Result>[] array, int K)
        {
            List<Result> r = new List<Result>(K);
            int i, j, max;
            Task<Result> temp;
            for (i = 0; i < K; i++)
            {
                max = i;
                for (j = i + 1; j < array.Length; j++)
                {
                    if (array[j].Result.score > array[max].Result.score)
                    {
                        max = j;
                    }
                }
                //add to our best
                r.Add(array[max].Result);
                //switchero so we don't iterate over this element again
                temp = array[i];
                array[i] = array[max];
                array[max] = temp;
                
            }
            return r;
        }
    }
}
