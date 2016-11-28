using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Tetris;
namespace Tetris.Controls
{
    /// <summary>
    /// Interaction logic for TileControl.xaml
    /// </summary>
    public partial class TileControl : UserControl
    {
        public int NumTiles { get; private set; }
        public Algorithms.Shape Shape { get; set; }

        public bool IsValid { get; set; }

        public bool IsDuplicate { get; set; }

        static System.Drawing.Color BACKGROUND = System.Drawing.Color.FromArgb((byte)0, (byte)(255), (byte)(255), (byte)255);
        public TileControl()
        {
            InitializeComponent();
            IsValid = true;
            IsDuplicate = false;
        }

        private void AdjustForValidationResult (int ValidValue)
        {
            //1-tile is hollow, 2-tile is disconnected, 3-tile is empty
            
            if (ValidValue == 1)
            {
                AdjustControlForInvalidTile("Hole");
                this.NumTiles = 0;
                IsValid = false;
            }
            else if (ValidValue == 2)
            {
                AdjustControlForInvalidTile("Disconnected");
                this.NumTiles = 0;
                IsValid = false;
            }
            else if (ValidValue == 3)
            {
                AdjustControlForInvalidTile("Empty");
                this.NumTiles = 0;
                IsValid = false;
            }
        }
        public void Increment()
        {
            this.NumTiles++;
            this.TilesAmountBox.Text = this.NumTiles.ToString();
        }

        public void Decrement()
        {
            this.NumTiles--;
            if (this.NumTiles < 0)
                this.NumTiles = 0;
            this.TilesAmountBox.Text = this.NumTiles.ToString();
        }

        private void AdjustControlForInvalidTile(string message)
        {
            this.TileLabel.Content = message;
            this.TilesAmountBox.Text = "0";
            this.TilesAmountBox.Foreground = System.Windows.Media.Brushes.Orange;
            this.PlusButton.Visibility = Visibility.Hidden;
            this.MinusButton.Visibility = System.Windows.Visibility.Hidden;
            this.TileLabel.Foreground = System.Windows.Media.Brushes.Orange;
            this.BorderBrush = System.Windows.Media.Brushes.Orange;
        }
        public TileControl(byte[,] TileArray, ref List<Algorithms.Shape> Shapes, System.Drawing.Color Col)
        {
            InitializeComponent();
            NumTiles = 1;
            this.TileImage.Source = BitmapToImageSource(GetTileBitmap(TileArray, ref Shapes, Col));
            this.TileImage.SnapsToDevicePixels = true;
        }
        //constructor from existing shape
        public TileControl(Algorithms.Shape s)
        {
            InitializeComponent();
            NumTiles = 1;
            this.TileImage.Source = BitmapToImageSource(GetTileBitmap(s));
            this.TileImage.SnapsToDevicePixels = true;
            this.Shape = s;
            IsValid = true;
            IsDuplicate = false;
            AdjustForValidationResult(Algorithms.ShapeValidator.isTileValid(this.Shape));

        }

        public void MarkAsDuplicate ()
        {
            AdjustControlForInvalidTile("Duplicate");
            this.TilesAmountBox.Foreground = System.Windows.Media.Brushes.Red;
            this.TileLabel.Foreground = System.Windows.Media.Brushes.Red;
            this.BorderBrush = System.Windows.Media.Brushes.Red;
            this.NumTiles = 0;
            IsDuplicate = true;
            IsValid = false;
        }

        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private System.Drawing.Bitmap GetTileBitmap(Algorithms.Shape s)
        {
            int width = s.rotations[0].GetLength(0);
            int height = s.rotations[0].GetLength(1);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (s.rotations[0][x, y] == 1)
                    {
                        if ((x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0))
                        {
                            bitmap.SetPixel(x, y, s.c1);
                        }
                        else
                        {
                            bitmap.SetPixel(x, y, s.c2);
                        }
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, BACKGROUND);
                    }
                }
            }
            return bitmap;
        }

        private System.Drawing.Bitmap GetTileBitmap(byte[,] TileArray, ref List<Algorithms.Shape> Shapes, System.Drawing.Color Col)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(TileArray.GetLength(0), TileArray.GetLength(1));
            System.Drawing.Color c2 = System.Drawing.Color.FromArgb((byte)0, (byte)((Col.R + (byte)20) > 255 ? 255 : (byte)(Col.R + (byte)20)), (Col.G + (byte)20) > 255 ? 255 : (byte)(Col.G + (byte)20), (Col.B + (byte)20) > 255 ? 255 : (byte)(Col.B + (byte)20));
            //both tile colors are defined here, the proper (and not intuitive...) place to create a shape
            Shapes.Add(new Algorithms.Shape(TileArray, Col, c2));

            for (int x = 0; x < TileArray.GetLength(0); x++)
            {
                for (int y = 0; y < TileArray.GetLength(1); y++)
                {
                    if (TileArray[x, y] == 1)
                    {
                        if ((x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0))
                        {
                            bitmap.SetPixel(x, y, Col);
                        }
                        else
                        {
                            bitmap.SetPixel(x, y, c2);
                        }
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, BACKGROUND);
                    }
                }
            }
            return bitmap;
        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {
            this.NumTiles--;
            if (this.NumTiles<0)
            this.NumTiles = 0;
            this.TilesAmountBox.Text = this.NumTiles.ToString();
        }
        private void PlusClick(object sender, RoutedEventArgs e)
        {
            this.NumTiles++;
            this.TilesAmountBox.Text = this.NumTiles.ToString();
        }

        private void SizeChange(object sender, SizeChangedEventArgs e)
        {
            this.Height = this.ActualWidth;
        }

    }
}
