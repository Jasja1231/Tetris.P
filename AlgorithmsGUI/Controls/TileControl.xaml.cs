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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgorithmsGUI.Controls
{
    /// <summary>
    /// Interaction logic for TileControl.xaml
    /// </summary>
    public partial class TileControl : UserControl
    {
        public int NumTiles { get; private set; }
        public TileControl()
        {
            InitializeComponent();
        }
        public TileControl(byte[,] TileArray, System.Drawing.Color Col)
        {
            InitializeComponent();
            NumTiles = 1;
            this.TileImage.Source = BitmapToImageSource(GetTileBitmap(TileArray, Col));
            this.TileImage.SnapsToDevicePixels = true;
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

        private System.Drawing.Bitmap GetTileBitmap(byte[,]TileArray)
        {
            Random r = new Random();
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(TileArray.GetLength(1), TileArray.GetLength(0));
            System.Drawing.Color background =  System.Drawing.Color.FromArgb((byte)0, (byte)(255), (byte)(255), (byte)255);
            System.Drawing.Color c =  System.Drawing.Color.FromArgb((byte)0, (byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
            System.Drawing.Color c2 =  System.Drawing.Color.FromArgb((byte)0, (byte)(c.R + (byte)20), (byte)(c.G + (byte)20), (byte)(c.B + (byte)20));
            for (int x = 0; x < TileArray.GetLength(0);x++)
            {
                for (int y=0; y< TileArray.GetLength(1);y++)
                {
                    if (TileArray[x,y]==1)
                    {
                       if ((x%2==0 && y%2 ==0)||(x%2!=0 && y%2 !=0))
                       {
                           bitmap.SetPixel(x,y,c);
                       }
                       else
                       {
                           bitmap.SetPixel(x,y,c2);
                       }
                    }
                    else
                    {
                        bitmap.SetPixel(x,y,background);
                    }
                }
            }
                return bitmap;  
        }

        private System.Drawing.Bitmap GetTileBitmap(byte[,] TileArray, System.Drawing.Color Col)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(TileArray.GetLength(0), TileArray.GetLength(1));
            System.Drawing.Color background = System.Drawing.Color.FromArgb((byte)0, (byte)(255), (byte)(255), (byte)255);
            System.Drawing.Color c2 = System.Drawing.Color.FromArgb((byte)0, (byte)((Col.R + (byte)20) > 255 ? 255 : (byte)(Col.R + (byte)20)), (Col.G + (byte)20) > 255 ? 255 : (byte)(Col.G + (byte)20), (Col.B + (byte)20) > 255 ? 255 : (byte)(Col.B + (byte)20));
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
                        bitmap.SetPixel(x, y, background);
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
