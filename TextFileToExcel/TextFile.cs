using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileToExcel
{
    public class TextFile
    {
        private string fileName;
        private string filePath;

        public TextFile(string path)
        {
            if(File.Exists(path))
            {
                filePath = path;
                fileName = Path.GetFileName(path);
            }
            else
            {
                Console.WriteLine("File does not exist! Please reference an existing file!");
            }
        }

        public void OpenFileForReading()
        {
            try
            {
                File.OpenRead(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem opening the file for reading!");
                throw e;
            }
        }

        //public int GetValuesPerRow()
        //{
        //    using (var streamReader = File.OpenText(filePath))
        //    {
        //        string[] lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray());
        //    }

        //}

    }
}
