using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Algorithms
{
   public class FileReader
    {
        //load shapes and mainTable width from file
        public static Tuple<List <byte [,]>,int> GetBricksFromFile (string [] FileContent)
        {
            List<byte[,]> Bricks = new List<byte[,]>();
            string [] spacecep = new string [] {" "};
            int tablewidth = Int32.Parse(FileContent[0].Split(spacecep,StringSplitOptions.None)[0]);
            int width, height;
            for (int i = 1; i < FileContent.Length; i++)
            {
                //read width of the table and set height to number of tiles red from first line of a file
                string[] linecontent = FileContent[i].Split(spacecep, StringSplitOptions.None);
                width = Int32.Parse(linecontent[0]);
                height = Int32.Parse(linecontent[1]);
                Byte[,] arr = new byte[width, height];
                i++;
                //write shape tiles into byteArray
                for (int j =0; j < height; j++)
                {
                    linecontent = FileContent[j+i].Split(spacecep, StringSplitOptions.None);

                    for (int k=0; k<width; k++)
                    {
                        byte val = Byte.Parse(linecontent[k]);
                        arr[k,j] = val;
                    }
                }
                i += height - 1;
                Bricks.Add(arr);
            }
                return new Tuple<List<byte[,]>,int>(Bricks,tablewidth);
        }
    }
}
