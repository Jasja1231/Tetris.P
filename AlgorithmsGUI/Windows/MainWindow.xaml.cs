using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris;
using Tetris.Algorithms;
using Tetris.Controllers;

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
        Model model;

        Controller controller;

        //*********************************CLASS METHODS***************************************/
        public MainWindow(Model alg, Controller control)
        {
            InitializeComponent();

            model = alg;
            controller = control;
            controller.setView(this);

            FFStepSetter.SelectedValue = 2;
            KSetter.SelectedValue = 4;
            AddBitMaps();
        }

        private void AddBitMaps()
        {
            System.Drawing.Bitmap bitmap;
            this.WellsPanel.Children.Clear();
            int x = 50;
            
            for (int k = 0; k < model.K; k++)
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

                if (this.model.Shapes.Count != 0)
                {
                    /*//plase 50 random shapes on board
                    Random r = new Random();
                    for (int i = 0; i < 50; i++)
                    {
                        this.AddTileToBitmap(ref bitmap, ShapesInfoListWrapper.ElementAt(r.Next(ShapesInfoListWrapper.Count - 1)),
                            r.Next(0, x - 5), r.Next(0, 75 - 5), r.Next(3), r);
                    }*/
                    foreach (Shape s in model.Shapes)
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
            //start tasks with (LongRunning) work
            int numOfTasks = model.K;
            Task<Result>[] tasks = new Task<Result>[numOfTasks];
            Result[] results = new Result[numOfTasks];
            string display = "Results\n";
            for (int i = 0; i < numOfTasks; i++)
            {
                //create stubs MainTable/Shape
                MainTable mt = new MainTable(i);
                Shape s = new Shape();
                tasks[i] = Task<Result>.Factory.StartNew(() =>
                {
                    FindGoodPlacement fpg = new FindGoodPlacement();
                    return fpg.work(mt, s);
                }, TaskCreationOptions.LongRunning);
            }
            Task.WaitAll(tasks);
            //collect results
            for (int i = 0; i < numOfTasks; i++)
            {
                results[i] = tasks[i].Result;
                display += "MainTable=" + results[i].Kth + ":(" + results[i].x + "," + results[i].y + ")\n";
            }
            MessageBox.Show(display);
        }

        //on click handler for "show tile browser" button
        private void ShowTileBrowser(object sender, RoutedEventArgs e)
        {
            if (model.Shapes.Count != 0)
            {
                TileBrowser TB = new TileBrowser(model.Shapes);
                if (TB.ShowDialog() == true)
                {
                    controller.ApplyShapes(TB.TileControls);
                }
            }
            else
            {
                MessageBox.Show("No file has been loaded");
            }
        }


        /// <summary>
        ///On click handler for "load file" button that asks controller to load from selected file.
        ///Creates and opens TileBrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            //List<byte[,]> Tiles;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open file";
            if (theDialog.ShowDialog() == true)
            {
                //ask controller to load selected file
                bool loaded = this.controller.LoadFromFile(theDialog.FileName);

                if (loaded == false)
                {
                    MessageBox.Show("Error occured while loading the selected file.");
                }
                else {
                    TileBrowser TB = new TileBrowser(model.Shapes);
                    if (TB.ShowDialog() == true)
                    {
                        controller.ApplyShapes(TB.TileControls);
                    }
                }
            }

            

        }

        //on click handler for "play" button
        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (String.Equals(this.PlayButton.Content, "Play") == true)
            {
                //Check if computation is started for the first time
                if (this.model.ComputationStarted == false)
                    this.controller.StartComputation(KSetter.SelectedValue);
                //Or it was paused and we need to resume it
                else
                    this.controller.ResumePausedComputation();
            }
            else
                this.controller.PauseComputation();

            this.PlayButton.Content = String.Equals(this.PlayButton.Content, "Play") ? "Pause" : "Play";
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

        private void UpdateImageTest(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            List<ImageSource> list = new List<ImageSource>();
            List<Result> results = new List<Result>();
            foreach(UIElement el in this.WellsPanel.Children)
            {
                Image im = (Image)el;
                list.Add(im.Source);
            }
            Result res = new Result(model.Shapes.ElementAt(r.Next (0,model.Shapes.Count-1)), r.Next(0,40), r.Next(0,300), r.Next(0,this.WellsPanel.Children.Count-1), 0);
            res.rotation = 0;
            results.Add(res);
            var updated = ImageSomething.UpdateImages(results, list);
            Image imm = new Image();
            imm.Source = updated.ElementAt(0);
            this.WellsPanel.Children.Add(imm);
            int x;

        }
    }
}
