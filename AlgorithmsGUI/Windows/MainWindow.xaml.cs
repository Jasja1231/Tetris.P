using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris;
using Tetris.Algorithms;

namespace Tetris.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Tetris.ObserverDP.Observer
    {
        //*********************************CLASS FIELDS****************************************/
        /// <summary>
        /// Algorithm - logic of the application. Subject for MainVindow.
        /// </summary>
        AlgorithmMain algorithm;


        //*********************************CLASS METHODS***************************************/
        public MainWindow(AlgorithmMain alg)
        {
            InitializeComponent();
            algorithm = alg;
            FFStepSetter.SelectedValue = 2;
            KSetter.SelectedValue = 4;
            AddBitMaps();
        }

        private void AddBitMaps()
        {
            System.Drawing.Bitmap bitmap;
            this.WellsPanel.Children.Clear();
            int x = 50;
            
            for (int k = 0; k < algorithm.K; k++)
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

                if (this.algorithm.Shapes.Count != 0)
                {
                    /*//plase 50 random shapes on board
                    Random r = new Random();
                    for (int i = 0; i < 50; i++)
                    {
                        this.AddTileToBitmap(ref bitmap, Shapes.ElementAt(r.Next(Shapes.Count - 1)),
                            r.Next(0, x - 5), r.Next(0, 75 - 5), r.Next(3), r);
                    }*/
                    foreach (Shape s in algorithm.Shapes)
                    {
                        this.AddTileToBitmap(ref bitmap, s, 0, 0, 0);
                    }
                }

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


        private void AddTileToBitmap (ref System.Drawing.Bitmap Bm, Shape Sh, int x, int y, int rotation)
        {
            int ShWidth = Sh.rotations.ElementAt(rotation).GetLength(0) - 1;
            int ShHeight = Sh.rotations.ElementAt(rotation).GetLength(1) - 1;

            for (int i = x, i2 = 0; i <= x + ShWidth; i++, i2++)
            {
                for (int j = Bm.Height - 1 - y, j2 = ShHeight; j >= Bm.Height - 1 - y - ShHeight; j--, j2--)
                {
                    if (Sh.rotations.ElementAt(rotation)[i2, j2] == 1)
                    {
                        Bm.SetPixel(i, j, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? Sh.c1 : Sh.c2);
                    }
                }
            }
        }


        //*******************************ON CLICK HANDLERS**************************************/
        private void TestThreads(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < algorithm.K; i++)
            {
                ThreadWorker tw = new ThreadWorker();
                tw.InitializeThreadWorker();
                FindGoodPlacement fpg = new FindGoodPlacement();
                tw.bw.RunWorkerAsync(fpg);
                //ThreadPool.QueueUserWorkItem(tw.DoWork, )
            }
        }

        //on click handler for "show tile browser" button
        private void ShowTileBrowser(object sender, RoutedEventArgs e)
        {
            if (algorithm.Shapes.Count != 0)
            {
                TileBrowser TB = new TileBrowser(algorithm.Shapes);
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
                    TileBrowser TB = new TileBrowser(Tiles, /*ref*/ algorithm.Shapes);
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


        /// <summary>
        /// Method that updates GUI when you call Notify(arg) from AlgorithmMain
        /// </summary>
        /// <param name="arg"></param>
        public void Update(int arg)
        {
            throw new NotImplementedException();
        }
    }
}
