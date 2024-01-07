using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZipExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseForZipButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.InitialDirectory = @"C:\";
            dialog.DefaultExt = ".zip";
            dialog.Filter = "Zip files (.zip)|*.zip";

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                zipFileTextBox.Text = dialog.FileNames[0];
                destPathTextBox.Text = GetDefaultExtractionPath();
            }
        }

        private string GetDefaultExtractionPath()
        {
            string[] substrings = zipFileTextBox.Text.Split('\\');
            string daZip = substrings.Last();
            daZip = daZip.Replace(".zip", "");


            string newExtractPath = "";
            for(int i = 0; i < substrings.Length - 1; i++)
            {
                newExtractPath += substrings[i];
                newExtractPath += "\\";
            }
            newExtractPath += daZip;

            return newExtractPath;
        }

        private void BrowseForDestPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            //if(result == DialogResult.OK)   
            destPathTextBox.Text = dialog.SelectedPath;


        }

        private void extractButton_Click(object sender, RoutedEventArgs e)
        {
            if(File.Exists(zipFileTextBox.Text) && Directory.Exists(destPathTextBox.Text) && IsDirectoryEmpty(destPathTextBox.Text))
            {
                ExtractFile(zipFileTextBox.Text, destPathTextBox.Text);
            }
        }

        public static void ExtractFile(string zipPath, string extractPath)
        {
            // Extract current zip file
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            string[] allNestedEntries = Directory.GetFileSystemEntries(extractPath, "*", SearchOption.AllDirectories);

            for (int i = 1; i < allNestedEntries.Length; i++)
            {
                FileInfo f = new FileInfo(allNestedEntries[i]);
                string filePath = f.FullName;
                string[] substrings = filePath.Split('\\');
                string newPath = "";

                for (int j = 0; j < substrings.Length - 2; j++)
                {
                    newPath += substrings[j];
                    newPath += "\\";
                }
                newPath += substrings[substrings.Length - 1];

                // If current entry is a file
                if (File.Exists(allNestedEntries[i]))
                    File.Move(allNestedEntries[i], newPath);
                // Else if current entry is a directory
                else if (Directory.Exists(allNestedEntries[i]))
                    Directory.Move(allNestedEntries[i], newPath);
            }

            Directory.Delete(allNestedEntries[0]);

            //// Enumerate nested zip files
            IEnumerable<string> nestedZipFiles = Directory.EnumerateFiles(extractPath, "*.zip", SearchOption.AllDirectories);

            // iterate the enumerator
            foreach (string nestedZipFile in nestedZipFiles)
            {
                // Get the nested zip full path + it's file name without the ".zip" extension
                // I.E this "C:\users\YourUserName\Documents\SomeZipFile.Zip" - turns to this: "C:\users\YourUserName\Documents\SomeZipFile".
                string nestedZipExtractPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(nestedZipFile), System.IO.Path.GetFileNameWithoutExtension(nestedZipFile));

                // extract recursively
                ExtractFile(nestedZipFile, nestedZipExtractPath);
                File.Delete(nestedZipFile);
            }
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}
