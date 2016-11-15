using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tetris.Windows;
using Tetris.Algorithms;
using Tetris.Controllers;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Model model = new Model();
            Controller controller = new Controller(model);
            MainWindow mainView = new MainWindow(model, controller);
            mainView.Show();
        }
    }
}
