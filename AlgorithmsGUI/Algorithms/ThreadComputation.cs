using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Algorithms
{
    class ThreadComputation
    {
        //*********************************CLASS FIELDS****************************************/
        private volatile Boolean work;
        private BackgroundWorker bgWorker;
        private Model m;
        private Model model;
        //*********************************CLASS METHODS***************************************/
        public ThreadComputation(Model m)
        {
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(preformIteration);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(passResult);
            this.m = m;
        }


        public void getNextIteration(Model m, int K, List<MainTable> lmt, int iter)
        {
            work = true;
            Args args = new Args(m, K, lmt, iter);
            bgWorker.RunWorkerAsync(args);
            /*List<Result> results = new List<Result>();
            Thread worker = new Thread(() => { results = preformIteration(m, K, lmt, iter); });
            worker.Start();*/
        }
        public void preformIteration(object sender, DoWorkEventArgs a)
        {
            Args args = (Args)a.Argument;
            List<Result> results = new List<Result>();
            int iteration = 0;
            List<Result> bestResults = new List<Result>(args.K);
            while (work && args.iter > iteration)
            {
                //start tasks with (LongRunning) work
                int nonZeroElements = args.m.ShapeQuantities.Count(x => x != 0);
                int numOfTasks = args.lmt.Count * nonZeroElements;
                Task<Result>[] tasks = new Task<Result>[numOfTasks];
                FindGoodPlacement fpg = new FindGoodPlacement();
                //string display = "Best K results\n";

                //for each MAIN TABLE == from 0 until K
                for (int i = 0; i < args.lmt.Count; i++)
                {
                    MainTable mt = args.lmt.ElementAt(i);
                    //for each unique! shape
                    for (int j = 0; j < mt.Quantities.Length; j++)
                    {
                        //Shape temp = sil.GetShapeAt(i);
                        //CREATE THREAD to find its position and start its THREAD_WORK
                        if (mt.Quantities[j] != 0)
                        {
                            int kurwa = j;
                            tasks[(mt.Quantities.Length * j + i) > 0 ? mt.Quantities.Length * j + i - 1 : mt.Quantities.Length * j + i] = Task<Result>.Factory.StartNew(() =>
                            {
                                return fpg.work(args.m, mt, kurwa);
                            }, TaskCreationOptions.LongRunning);
                        }
                    }
                }
                //BLOCK UNTILL ALL THREADS FINNISH
                Task.WaitAll(tasks);
                //Copy K best results into our list of best results(MAIN TABLES?)
                bestResults = SelectionSort(tasks, args.K);
                /*for (int i = 0; i < K; i++)
                {
                    display += "MainTable=" + bestResults.ElementAt(i).Kth + ":(" + bestResults.ElementAt(i).x + "," +
                       bestResults.ElementAt(i).y + "), score=" + bestResults.ElementAt(i).score + "\n";
                } 
                MessageBox.Show(display); DEBUG */
                iteration++;
            }
            args.m.RemainingShapes--;
            a.Result = bestResults;
        }

        //Complexity O(n) not O(n^2) since I only find K largest :)
        private List<Result> SelectionSort(Task<Result>[] array, int K)
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

        private void passResult(object sender, RunWorkerCompletedEventArgs e)
        {
           // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                MessageBox.Show("Cancelled");
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                List<Result> r = (List<Result>)e.Result;
                m.AddBestResults(r);
            }
            // Enable controls?
        }

        public void pauseComputation()
        {
            work = false;
        }

        private class Args
        {
            public Model m;
            public int K;
            public List<MainTable> lmt;
            public int iter;
            public Args(Model m, int K, List<MainTable> lmt, int iter)
            {
                this.m = m;
                this.K = K;
                this.lmt = lmt;
                this.iter = iter;
            }
        }
    }
}
