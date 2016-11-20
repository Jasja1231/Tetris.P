

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
                 else if (obj is List<Result>) 
                 {
                     dupa = JsonConvert.SerializeObject((List<Result>)obj);
                     //write to file
                     System.IO.File.WriteAllText(filepath + "bestResultsListJSON", dupa);
                 }
                 else if (obj is Shape[])
                 {
                     dupa = JsonConvert.SerializeObject((Shape[])obj);
                     //write to file
                     System.IO.File.WriteAllText(filepath + "shapesDatabaseListJSON", dupa);
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

         /// <summary>
         /// Deserializes MainWindow list from provided path. If Non parameters are givern
         /// </summary>
         /// <param name="o">takes objects that you want to deserialize into and filepath.
         /// Order of parameters matter : list pf main tables shoud be put before list of bitmaps.</param>
         public static void Deserialize(params Object[] o)
         {
             string filepath = "";
             int bitmapCount = 0;
             List<ImageSource> tempimgS = new List<ImageSource>();
             //for (Object obj in o)
             for (int i = 0; i < o.Count();i++ )
             {
                 if (o[i] is string)
                 {
                     filepath = filepath + (string)o[i] + Path.PathSeparator;
                 }
                 else if (o[i] is List<MainTable>)
                 {
                     //Read file content to deserialize into list of main tables 
                     
                     string fileContent = System.IO.File.ReadAllText(filepath + "mainTablesListJSON");
                     o[i] = (List<MainTable>)JsonConvert.DeserializeObject(fileContent);
                     List<MainTable> mt = (List<MainTable>)o[i];
                     bitmapCount = mt.Count();

                     //read bitmaps - CHECK IF NOT bitmapCount is k or k+1
                     for (int k = 0; k < bitmapCount; k++)
                     {
                         BitmapImage bitmap = new BitmapImage(new Uri(filepath + k.ToString() + "K.png"));
                         ImageSourceConverter c = new ImageSourceConverter();
                         ImageSource ims = (ImageSource)c.ConvertFrom(bitmap);
                         tempimgS.Add(ims);
                     }
                 }
                 else if (o[i] is List<Result>)
                 {
                     string fileContent = System.IO.File.ReadAllText(filepath + "bestResultsListJSON");
                     o[i] = (List<Result>)JsonConvert.DeserializeObject(fileContent);
                 }
                 else if (o[i] is Shape[])
                 {

                     string fileContent = System.IO.File.ReadAllText(filepath + "shapesDatabaseListJSON");
                     o[i] = (Shape[])JsonConvert.DeserializeObject(fileContent);
                 }
                 else if (o[i] is List<ImageSource>)
                 {
                     List<ImageSource> imgS = (List<ImageSource>)o[i];
                     imgS.Clear();
                     imgS = tempimgS;
                 }
             }
         }
     }
 }
