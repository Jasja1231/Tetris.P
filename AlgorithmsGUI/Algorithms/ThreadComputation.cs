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
        private Model m;
        private Args args;
        //*********************************CLASS METHODS***************************************/
        public ThreadComputation(Model m)
        {
            this.m = m;
        }

        public void preformIteration( Model m, int K, List<MainTable> lmt)
        {
            //TODO: disable controls
            args = new Args(m, K, lmt);
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(preformIteration);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(passResult);
            bgWorker.RunWorkerAsync(args);
        }

        public void preformIteration(object sender, DoWorkEventArgs a)
        {
            Args args = (Args)a.Argument;
            List<Result> results = new List<Result>();
            int iteration = 0;
            List<Result> bestResults = new List<Result>(args.K);
            
                //start tasks with (LongRunning) work
                int sumNonZeroElements = 0;
                for(int i = 0; i < args.lmt.Count; i++)
                {
                    MainTable mt = args.lmt.ElementAt(i);
                    sumNonZeroElements += mt.Quantities.Count(x => x != 0);
                }
                //int numOfTasks = args.lmt.Count * sumNonZeroElements;
                Task<Result>[] tasks = new Task<Result>[sumNonZeroElements];
                FindGoodPlacement fpg = new FindGoodPlacement();
                //string display = "Best K results\n";

                //for each MAIN TABLE == from 0 until K
                int taskIdx = 0;
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
                            tasks[taskIdx++] = Task<Result>.Factory.StartNew(() =>
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
                //for (int i = 0; i < args.K; i++)
                //{
                //    display += "MainTable=" + bestResults.ElementAt(i).Kth + ":(" + bestResults.ElementAt(i).x + "," +
                //       bestResults.ElementAt(i).y + "), score=" + bestResults.ElementAt(i).score + "\n";
                //} 
                //MessageBox.Show(display);
                iteration++;
            
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
            // TODO: Enable controls?
        }

        private class Args
        {
            public Model m;
            public int K;
            public List<MainTable> lmt;
            public Args(Model m, int K, List<MainTable> lmt)
            {
                this.m = m;
                this.K = K;
                this.lmt = lmt;
            }
        }
    }
}
