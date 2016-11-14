using System.Windows;

namespace Tetris.Algorithms
{
    class ThreadWorker
    {
        //*********************************CLASS FIELDS****************************************/
        public System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
        //*********************************CLASS METHODS***************************************/
        public void InitializeThreadWorker()
        {
            // Attach event handlers to the BackgroundWorker object.
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(DoWork);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(work_completed);
        }

        public void DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FindGoodPlacement fgp = (FindGoodPlacement)e.Argument;
            // Return the value through the Result property.
            //e.Result = fgp.work();
        }

        public void work_completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            int sleeptime = (int)e.Result;
            MessageBox.Show("Worker sleep for: " + sleeptime.ToString());
        }
    }
}
