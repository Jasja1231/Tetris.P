using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlgorithmsGUI.Controls;

namespace AlgorithmsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TileBrowser : Window
    {
        public TileBrowser()
        {
            InitializeComponent();
        }

        public TileBrowser(List<byte[,]> Tiles)
        {
            InitializeComponent();
            Random r = new Random();
            foreach (byte[,] b in Tiles)
            {
                TileControl t = new TileControl(b, System.Drawing.Color.FromArgb(0, (byte)r.Next(20,235), (byte)r.Next(20,235), (byte)r.Next(20,235)));
                this.TilesPanel.Children.Add(t);
            }

        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.TilesPanel.Columns = (int)(this.ActualWidth / 230);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.TilesPanel.Columns = (int)(this.ActualWidth / 230);

        }
    }
}
