using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            AddBitMaps();
        }

        private void AddBitMaps()
        {
            System.Drawing.Bitmap bitmap;
            this.WellsPanel.Children.Clear();
            int x = 50;

            for (int k = 0; k < 5; k++)
            {
                Image im = new Image();
    
                    bitmap = new System.Drawing.Bitmap(x, 75);
                    System.Drawing.Color c1 = System.Drawing.Color.FromArgb(0, 220, 220, 220);
                    System.Drawing.Color c2 = System.Drawing.Color.FromArgb(0, 230, 230, 230);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            bitmap.SetPixel(i, j, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? c1 : c2);
                        }
                    }

                    if (this.Tiles != null)
                    {
                        Random r = new Random();
                      
                        for (int i = 0; i < 50; i++)
                        {
                            Tetris.shape s = new Tetris.shape(Tiles.ElementAt(r.Next(Tiles.Count - 1)));
                            this.AddTileToBitmap(ref bitmap, s, r.Next(0, x - 5), r.Next(0, 75 - 5), r.Next(3), r);
                        }
                    }

              /*  else
                {
                    bitmap = new System.Drawing.Bitmap(100, 200);
                    System.Drawing.Color c1 = System.Drawing.Color.FromArgb(0, 100, 100, 100);
                    System.Drawing.Color c2 = System.Drawing.Color.FromArgb(0, 120, 120, 120);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            bitmap.SetPixel(i, j, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? c1 : c2);
                        }
                    }
                }*/

                BitmapImage bitmapimage = new BitmapImage();

                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;

                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                }

                im.Source = bitmapimage;
                this.WellsPanel.Children.Add(im);

            }
        }
        private void AddTileToBitmap (ref System.Drawing.Bitmap Bm, Tetris.shape Sh, int x, int y, int rotation, Random r)
        {
           // Random r = new Random();
            System.Drawing.Color c1 = System.Drawing.Color.FromArgb(0, r.Next(20, 230), r.Next(20, 230), r.Next(20, 230));
            System.Drawing.Color c2 = System.Drawing.Color.FromArgb(0, c1.R + 5, c1.G + 5, c1.B + 5);

            for (int i = x, i2 = 0; i< x + Sh.rotations.ElementAt(rotation).GetLength(0);i++,i2++)
            {
                for (int j = y + Sh.rotations.ElementAt(rotation).GetLength(1) - 1, j2=Sh.rotations.ElementAt(rotation).GetLength(1)-1; j >= y; j--,j2--)
                {
                    if (Sh.rotations.ElementAt(rotation)[i2,j2] == 1)
                    {
                        Bm.SetPixel(i,j, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? c1 : c2);
                    }
                }
            }
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
            AddBitMaps();

        }

        private void ZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            var st = (ScaleTransform)WellsPanel.LayoutTransform;
            double zoom = e.NewValue;
            st.ScaleX = zoom * 2;
            st.ScaleY = zoom * 2;
        }
        
    }
}
