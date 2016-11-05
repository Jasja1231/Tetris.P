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
using Tetris;

namespace AlgorithmsGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //*********************************CLASS FIELDS****************************************/
        public List<Shape> Shapes = new List<Tetris.Shape>();
        int K = 5; //the user set K variable, constant for now
        //*********************************CLASS METHODS***************************************/
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
            
            for (int k = 0; k < K; k++)
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

                if (this.Shapes.Count != 0)
                {
                    //plase 50 random shapes on board
                    Random r = new Random();
                    for (int i = 0; i < 50; i++)
                    {
                        this.AddTileToBitmap(ref bitmap, Shapes.ElementAt(r.Next(Shapes.Count - 1)),
                            r.Next(0, x - 5), r.Next(0, 75 - 5), r.Next(3), r);
                    }
                }
                //TODO: what is this ? -KB
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
        private void AddTileToBitmap (ref System.Drawing.Bitmap Bm, Shape Sh, int x, int y, int rotation, Random r)
        {
           // Random r = new Random();
            for (int i = x, i2 = 0; i< x + Sh.rotations.ElementAt(rotation).GetLength(0);i++,i2++)
            {
                for (int j = y + Sh.rotations.ElementAt(rotation).GetLength(1) - 1, j2=Sh.rotations.ElementAt(rotation).GetLength(1)-1; j >= y; j--,j2--)
                {
                    if (Sh.rotations.ElementAt(rotation)[i2,j2] == 1)
                    {
                        Bm.SetPixel(i,j, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? Sh.c1 : Sh.c2);
                    }
                }
            }
        }
        //*******************************ON CLICK HANDLERS**************************************/
        //on click handler for "show tile browser" button
        private void ShowTileBrowser(object sender, RoutedEventArgs e)
        {
            if (Shapes.Count != 0)
            {
                TileBrowser TB = new TileBrowser(ref Shapes);
                TB.Show();
            }
            else
            {
                MessageBox.Show("No file has been loaded");
            }
        }
        //on click handler for "load file" button
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            List<byte[,]> Tiles;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open file";
            if (theDialog.ShowDialog() == true)
            {
                try
                {
                    string[] content = System.IO.File.ReadAllLines(theDialog.FileName);
                    Tiles = FileReader.GetBricksFromFile(content);
                    TileBrowser TB = new TileBrowser(Tiles, ref Shapes);
                    TB.Show();
                }
                catch
                {
                    MessageBox.Show("Error occured while loading the selected file.");
                }

            }
        }
        //on click handler for "play" button
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
