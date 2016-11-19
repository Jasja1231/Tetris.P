

using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using System.Windows.Controls;
 using System.Windows.Media.Imaging;
 //using Newtonsoft.Json;

namespace Tetris.Algorithms
 {
     class Serializer
     {
 
         private String DEFAULT_FILE_LOCATION = "";
 
         public static Serializer(params Object[] o) 
         {
             int bitmapCount = -1;
             int[] a = new int[133214];
             int asds = a.Count(x => x != 0);
             string dupa;
             foreach (Object obj in o) 
             {               
                 //if it is an image just save it to file
                 if(obj is Image){
                     bitmapCount +=1;
                     Image t = (Image)obj;
                     BitmapImage bi = (BitmapImage)t.Source;
 
                     BitmapEncoder encoder = new PngBitmapEncoder();
                     encoder.Frames.Add(BitmapFrame.Create(bi));
 
                     string fileName = bitmapCount.ToString() + ".png";
                     using (var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                     {
                         encoder.Save(fileStream);
                     }
                    
                 }
                 //if it is a model
                 else if (obj is Model) {
                     //dupa = JsonConvert.SerializeObject((Model)obj);     
                     //write to file
                     System.IO.File.WriteAllText("modelJSON",dupa);
                 }
             }
         }
     }
 }
