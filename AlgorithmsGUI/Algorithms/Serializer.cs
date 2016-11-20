

using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using System.Windows.Controls;
 using System.Windows.Media.Imaging;
using System.Windows.Media;
 using Newtonsoft.Json;
using System.IO;

namespace Tetris.Algorithms
 {
     class Serializer
     { 
         /// <summary>
         /// 
         /// </summary>
         /// <param name="o">Set of Objects.</param>
         public static void Serialize(params Object[] o) 
         {
             int bitmapCount = -1;
             string dupa;
             string filepath = "";
             foreach (Object obj in o) 
             {               
                 //if it is an image just save it to file
                 if (obj is List<ImageSource>)
                 {
                     foreach(ImageSource img in (List<ImageSource>)obj)
                     {
                         bitmapCount += 1;
                         BitmapImage bi = (BitmapImage)img;

                         BitmapEncoder encoder = new PngBitmapEncoder();
                         encoder.Frames.Add(BitmapFrame.Create(bi));

                         string fileName = bitmapCount.ToString() + "K.png";
                         using (var fileStream = new System.IO.FileStream(filepath + fileName, System.IO.FileMode.Create))
                         {
                             encoder.Save(fileStream);
                         }
                     }
                 }
                 //if it is a model
                 else if (obj is Model) {
                     dupa = JsonConvert.SerializeObject((Model)obj);     
                     //write to file
                     System.IO.File.WriteAllText(filepath+"modelJSON",dupa);
                 }
                 else if (obj is List<MainTable>)
                 {
                     dupa = JsonConvert.SerializeObject((List<MainTable>)obj);
                     //write to file
                     System.IO.File.WriteAllText(filepath + "mainTablesListJSON", dupa);
                 }
                 else if (obj is string) 
                 {
                     filepath = filepath + (string)obj + Path.PathSeparator;
                 }
             }
         }
     }
 }
