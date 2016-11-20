using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Algorithms;
using Tetris.Windows;

namespace Tetris.Controllers
{
    public class Controller
    {

        private Model model;
        private MainWindow view;
        public Controller(Model m) {
            this.model = m;
        }

        public void setView(Windows.MainWindow mainWindow)
        {
            this.view = mainWindow;
        }

        public bool LoadFromFile(string p)
        {
            return this.model.LoadFromFile(p);
        }

        public void ApplyShapes(List<Controls.TileControl> list)
        {
            this.model.ApplyShapes(list);
        }

        public void StartComputation(int p)
        {
            this.model.StartComputation(p);
        }

        public void PauseComputation()
        {
            this.model.PauseComputation();
        }

        internal void StartIteration(int p, int iter)
        {
            this.model.StartIteration(p, iter);
        }

        public void sendListOfImageSources(List<System.Windows.Media.ImageSource> isl)
        {
            this.model.ReadListOfImageSources(isl);
        }

        internal void StopComputation()
        {
            this.model.StopComputation();
        }
    }
}
