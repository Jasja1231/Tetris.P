

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
using System.Drawing;

namespace Tetris.Algorithms
{
    class Serializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">Set of Objects.</param>
        public static void Serialize(Model m, params Object[] o)
        {
            int bitmapCount = -1;
            string filepath = "";

            foreach (Object obj in o)
                if (obj is string)
                {
                    filepath = filepath + (string)obj + Path.DirectorySeparatorChar;
                }

            string dupa = JsonConvert.SerializeObject(m);
            //write to file
            System.IO.File.WriteAllText(filepath + "modelJSON", dupa);

            //Serialize Bitmaps 
            foreach (ImageSource img in m.ImageSources)
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

            //Serialize MainTables
            string mainTablesJSON = JsonConvert.SerializeObject(m.MainTablesList);
            //write to file
            System.IO.File.WriteAllText(filepath + "mainTablesListJSON", mainTablesJSON);

            //Seriaize Remaining shapes
            string remainingJSON = JsonConvert.SerializeObject(m.MainTablesList);
            //write to file
            System.IO.File.WriteAllText(filepath + "remainingShapesJSON", mainTablesJSON);

            //Serialise Best Results
            string bestResJSON = JsonConvert.SerializeObject(m.BestResults);
            //write to file
            System.IO.File.WriteAllText(filepath + "bestResultsListJSON", bestResJSON);


            //Serialize Shapes Database
            string shapeDataJSON = JsonConvert.SerializeObject(m.ShapesDatabase);
            //write to file
            System.IO.File.WriteAllText(filepath + "shapesDatabaseListJSON", shapeDataJSON);

        }




        /// <summary>
        /// Deserializes MainWindow list from provided path. If Non parameters are givern
        /// </summary>
        /// <param name="o">takes objects that you want to deserialize into and filepath.
        /// Order of parameters matter : list pf main tables shoud be put before list of bitmaps.</param>
        public static void Deserialize(Model m, params Object[] o)
        {
            string filepath = "";
            foreach (Object obj in o)
                if (obj is string)
                {
                    filepath = filepath + (string)obj + Path.DirectorySeparatorChar;
                }

            // deserialize  main tables list
            string fileContent = System.IO.File.ReadAllText(filepath + "mainTablesListJSON");
            m.MainTablesList = JsonConvert.DeserializeObject<List<MainTable>>(fileContent);

            //deserialize Image sources
            int bitMapCount = m.MainTablesList.Count;
            m.ImageSources.Clear();
            //read bitmaps - CHECK IF NOT bitmapCount is k or k+1
            for (int k = 0; k < bitMapCount; k++)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(filepath + k.ToString() + "K.png"));
                ImageSourceConverter c = new ImageSourceConverter();
                ImageSource ims = (ImageSource)bitmap;
                m.ImageSources.Add(ims);
            }


            //deseriaize Remaining shapes
            string remainingJSON = System.IO.File.ReadAllText(filepath + "remainingShapesJSON");  /// deserialize int from string txt 
            m.RemainingShapes = JsonConvert.DeserializeObject<int>(remainingJSON);

            //Deserilize Best Results
            string bestResJSON = System.IO.File.ReadAllText(filepath + "bestResultsListJSON");
            m.BestResults = JsonConvert.DeserializeObject<List<Result>>(bestResJSON);

            //deserialize Shapes Database
            string shapeDataJSON = System.IO.File.ReadAllText(filepath + "shapesDatabaseListJSON");
            m.ShapesDatabase = JsonConvert.DeserializeObject<Shape[]>(shapeDataJSON);

        }
    }
}
