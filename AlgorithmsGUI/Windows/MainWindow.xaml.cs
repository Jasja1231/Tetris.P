using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace AlgorithmsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<byte[,]> Tiles { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            FFStepSetter.SelectedValue = 2;
            KSetter.SelectedValue = 4;
        }

        private void ShowTileBrowser(object sender, RoutedEventArgs e)
        {
            if (Tiles != null)
            {
                TileBrowser TB = new TileBrowser(Tiles);
                TB.Show();
            }
            else
            {
                MessageBox.Show("No file has been loaded");
            }
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {

            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open file";
            if (theDialog.ShowDialog() == true)
            {
                try
                {

                    string[] content = System.IO.File.ReadAllLines(theDialog.FileName);
                    Tiles = FileReader.GetBricksFromFile(content);
                    TileBrowser TB = new TileBrowser(Tiles);
                    List<Tetris.shape> Shapes = new List<Tetris.shape>();
                    foreach(byte[,] b in Tiles)
                        {
                            Tetris.shape s = new Tetris.shape(b);
                            s.findAllRotations();
                            Shapes.Add(s);
                        }

                    for (int i = 0; i < Shapes.Count; i++ )
                    {
                        for (int j=0;j<Shapes.Count;j++)
                        {
                            if (i!=j)
                            {
                             //   foreach (byte [,] x in Shapes.ElementAt)
                                {

                                }
                            }
                        }
                    }
                        TB.Show();
                }
                catch
                {
                    MessageBox.Show("Error occured while loading the selected file.");
                }
            }
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            this.PlayButton.Content = String.Equals(this.PlayButton.Content, "Play") ? "Pause" : "Play";
        }        
    }
}
