using System;
using System.Collections.Generic;
using System.IO;

namespace rename
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            var osHelper = new OSHelper();
            var quit = false;
            while (!quit)
            {
                Console.WriteLine("input directory path:");
                var input = Console.ReadLine();
                Console.WriteLine("old string:");
                var oldString = Console.ReadLine();
                Console.WriteLine("new string:");
                var newString = Console.ReadLine();
                osHelper.Rename(input, oldString, newString);
            }
        }
    }

    public class OSHelper
    {
        public void Rename(string inputDir, string oldString, string newString)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(inputDir);
                var filter = new List<string>() { ".git", ".vs" };
                if (filter.Contains(dirInfo.Name)) return;

                var files = Directory.GetFiles(inputDir);
                foreach (var file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var content = File.ReadAllText(file);
                    if (content.Contains(oldString))
                        File.WriteAllText(file, content.Replace(oldString, newString));

                    if (fileInfo.Name.Contains(oldString))
                    {
                        var newName = fileInfo.Name.Replace(oldString, newString);
                        if (!string.IsNullOrEmpty(newName))
                            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + newName);
                        else Console.WriteLine("Empty name in file path after replacement!");
                    }
                }


                if (dirInfo.Name.Contains(oldString))
                {
                    var name = dirInfo.Name.Replace(oldString, newString);
                    if (!string.IsNullOrEmpty(name))
                        dirInfo.MoveTo(dirInfo.Parent.FullName + "\\" + name);
                    else Console.WriteLine("Empty name in directory path after replacement!");
                }

                var dirs = Directory.GetDirectories(inputDir);
                if (dirs.Length > 0)
                {
                    foreach (var dir in dirs)
                    {
                        Rename(dir, oldString, newString);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
