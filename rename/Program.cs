using System;
using System.IO;

namespace rename
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            var quit = false;
            while (!quit)
            {
                Console.WriteLine("input directory path:");
                var input = Console.ReadLine();
                Console.WriteLine("old string:");
                var oldString = Console.ReadLine();
                Console.WriteLine("new string:");
                var newString = Console.ReadLine();
                Rename(input, oldString, newString);
            }


            void Rename(string input, string oldString, string newString)
            {
                try
                {
                    var files = Directory.GetFiles(input);
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
                    var dirs = Directory.GetDirectories(input);
                    if (dirs.Length > 0)
                    {
                        foreach (var dir in dirs)
                        {
                            Rename(dir, oldString, newString);
                            DirectoryInfo dirInfo = new DirectoryInfo(dir);
                            if (dirInfo.Name.Contains(oldString))
                            {
                                var name = dirInfo.Name.Replace(oldString, newString);
                                if (!string.IsNullOrEmpty(name))
                                    dirInfo.MoveTo(dirInfo.Parent.FullName + "\\" + name);
                                else Console.WriteLine("Empty name in directory path after replacement!");
                            }

                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}
