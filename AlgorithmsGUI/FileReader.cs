using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsGUI
{
    class FileReader
    {

        public static List <byte [,]> GetBricksFromFile (string [] FileContent)
        {

            List<byte[,]> Bricks = new List<byte[,]>();
            string [] spacecep = new string [] {" "};
            int width, height;
            for (int i = 1; i < FileContent.Length; i++)
            {
                string[] linecontent = FileContent[i].Split(spacecep, StringSplitOptions.None);
                width = Int32.Parse(linecontent[0]);
                height = Int32.Parse(linecontent[1]);
                Byte[,] arr = new byte[width, height];
                i++;
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
                return Bricks;
        }
    }
}
