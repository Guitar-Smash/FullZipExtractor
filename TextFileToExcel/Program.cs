using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileToExcel
{
    public class Program
    {
        static void Main(string[] args)
        {
            string zipPath = @"C:\Users\JakeW\source\repos\SGBA.zip";
            string extractPath = @"C:\Users\JakeW\source\repos\SGBA";

            ExtractFile(zipPath, extractPath);
        }

        public static void ExtractFile(string zipPath, string extractPath)
        {
            Console.WriteLine(zipPath);

            if(Directory.Exists(extractPath))
                Directory.Delete(extractPath, true);

            Directory.CreateDirectory(extractPath);

            // Extract current zip file
            ZipFile.ExtractToDirectory(zipPath, extractPath);


            IEnumerable<string> allNestedEntries = Directory.EnumerateFileSystemEntries(extractPath, "*", SearchOption.AllDirectories);

            for(int i = 1; i < allNestedEntries.Count(); i++)
            {
                FileInfo f = new FileInfo(allNestedEntries.ElementAt(i));
                string filePath = f.FullName;
                string[] substrings = filePath.Split('\\');
                string newPath = "";

                for (int j = 0; j < substrings.Length - 1; j++)
                {
                    if (j < substrings.Length - 2)
                    {
                        newPath += substrings[j];
                        newPath += "\\";
                    }

                    else
                        newPath += substrings[substrings.Length - 1];
                }
                File.Move(allNestedEntries.ElementAt(i), newPath);
                Console.WriteLine(newPath);

                //Console.WriteLine(allNestedEntries.ElementAt(i));
            }





            // Enumerate nested zip files
            IEnumerable<string> nestedZipFiles = Directory.EnumerateFiles(extractPath, "*.zip", SearchOption.AllDirectories);

            // iterate the enumerator
            foreach (string nestedZipFile in nestedZipFiles)
            {
                // Get the nested zip full path + it's file name without the ".zip" extension
                // I.E this "C:\users\YourUserName\Documents\SomeZipFile.Zip" - turns to this: "C:\users\YourUserName\Documents\SomeZipFile".
                string nestedZipExtractPath = Path.Combine(Path.GetDirectoryName(nestedZipFile), Path.GetFileNameWithoutExtension(nestedZipFile));

                // extract recursively
                ExtractFile(nestedZipFile, nestedZipExtractPath);
                File.Delete(nestedZipFile);
            }
        }


    }
}
