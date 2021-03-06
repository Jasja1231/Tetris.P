﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris.Algorithms
{
    public static class ImageProcessor
    {
        //convert Bitmap to BitmapImage
        private static BitmapImage BitmapToBitmapSource(Bitmap bitmap)
        {
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
            return bitmapimage;
        }
        //convert BitmapImage to Bitmap
        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        //add given shape to existing bitmap
        private static void AddTileToBitmap(ref System.Drawing.Bitmap Bm, Shape Sh, int x, int y, int rotation)
        {
            int ShWidth = Sh.rotations.ElementAt(rotation).GetLength(0) - 1;
            int ShHeight = Sh.rotations.ElementAt(rotation).GetLength(1) - 1;
            //i = iterator for bitmap y's
            //j = iterator for bitmap x's
            //i2 = iterator for shape y's
            //j2 = iterator for shape x's
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
        //increase size of bitmap
        private static Bitmap CopytToExtendedBitmap(Bitmap original, int newwidth, int newheight)
        {
            Bitmap newbitmap = new Bitmap(newwidth, newheight);

            using (Graphics g = Graphics.FromImage(newbitmap))
            {
                g.DrawImage(original, 0, (newheight - original.Height), original.Width, original.Height);
            }

            return newbitmap;
        }
        //update images in all mainTables
        internal static BitmapImage[] UpdateImages (Model model, List<ImageSource>SourceImages)
        {
            var updated = new BitmapImage[model.K];
            int weLookAtImage = 0;
            foreach (Result result in model.BestResults)
            {
                Bitmap ToUpdate = BitmapImageToBitmap((BitmapImage)(SourceImages.ElementAt(result.Kth)));
                int newheight = result.y + model.ShapesDatabase[result.shapeIdx].rotations.ElementAt(result.rotation).GetLength(1);
                if (newheight > ToUpdate.Height)
                {
                    System.Drawing.Color c1 = System.Drawing.Color.FromArgb(0, 220, 220, 220);
                    System.Drawing.Color c2 = System.Drawing.Color.FromArgb(0, 230, 230, 230);
                    int oldheight = ToUpdate.Height;
                    ToUpdate = CopytToExtendedBitmap(ToUpdate, ToUpdate.Width, newheight);

                    bool darkerodd = (ToUpdate.Height - oldheight)  % 2 != 0 ? false : true;

                    for (int i=ToUpdate.Height - oldheight -1;i>=0;i--)
                    {
                        for (int j=0;j<ToUpdate.Width;j++)
                        {
                            if (darkerodd)
                                ToUpdate.SetPixel(j, i, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? c1 : c2);
                            else
                                ToUpdate.SetPixel(j, i, ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) ? c2 : c1);
                        }
                    }
                }
                AddTileToBitmap(ref ToUpdate, model.ShapesDatabase[result.shapeIdx], result.x, result.y, result.rotation);
                var newsource = BitmapToBitmapSource(ToUpdate);
                updated[weLookAtImage] =newsource;
                weLookAtImage++;
            }
            return updated;
        }
    }
}
