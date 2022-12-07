
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code.RockPaperScissorsTask;

namespace Advent_Of_Code
{
    internal class DirectoryNavigationTask
    {
        public static int FindDirSizeSumLT100k()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\07\input.txt"));

            //Read input
            string line;
            string[] lineArr;
            string currentPath = "";
            List<FileObject> files = new List<FileObject>();
            List<string> directories = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                lineArr = line.Split(" ");
                //Command
                if (lineArr[0] == "$")
                {
                    //Change directory
                    if (lineArr[1] == "cd" )
                    {
                        if (lineArr[2] == "..")
                        {
                            //Remove last directory from current path
                            currentPath = currentPath.Substring(0, currentPath.TrimEnd('/').LastIndexOf("/") + 1);
                        }
                        else
                        {
                            //Add directory to current path
                            if (currentPath.Length > 0)
                            {
                                currentPath += lineArr[2] + "/";
                            }
                            else
                            {
                                currentPath += lineArr[2];
                            }
                        }
                    }
                    //List contents
                    else if (lineArr[1] == "ls")
                    {

                    }
                } 
                //Directory
                else if (lineArr[0] == "dir")
                {
                    if (directories.Contains(currentPath + lineArr[1]) == false)
                    {
                        directories.Add(currentPath + lineArr[1]);
                    }
                }
                //File
                else if (int.TryParse(lineArr[0], out _))
                {
                    if (files.Count(f => f.path == currentPath && f.filename == lineArr[1]) == 0)
                    {
                        files.Add(new FileObject(int.Parse(lineArr[0]), lineArr[1], currentPath));
                    }
                }
            }

            //Sum of size for directories of size < 100000
            int sumDirSize = 0;
            foreach (string dir in directories)
            {
                int dirSize = 0;
                foreach (FileObject file in files.Where(f => f.path.StartsWith(dir))) 
                {
                    dirSize += file.filesize;
                }

                if (dirSize < 100000)
                {
                    sumDirSize += dirSize;
                }
            }

            return sumDirSize;
        }

        public static int FindSizeOfDirToDelete()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\07\input.txt"));

            //Read input
            string line;
            string[] lineArr;
            string currentPath = "";
            List<FileObject> files = new List<FileObject>();
            List<string> directories = new List<string>();
            directories.Add("/");
            while ((line = sr.ReadLine()) != null)
            {
                lineArr = line.Split(" ");
                //Command
                if (lineArr[0] == "$")
                {
                    //Change directory
                    if (lineArr[1] == "cd")
                    {
                        if (lineArr[2] == "..")
                        {
                            //Remove last directory from current path
                            currentPath = currentPath.Substring(0, currentPath.TrimEnd('/').LastIndexOf("/") + 1);
                        }
                        else
                        {
                            //Add directory to current path
                            if (currentPath.Length > 0)
                            {
                                currentPath += lineArr[2] + "/";
                            }
                            else
                            {
                                currentPath += lineArr[2];
                            }
                        }
                    }
                    //List contents
                    else if (lineArr[1] == "ls")
                    {

                    }
                }
                //Directory
                else if (lineArr[0] == "dir")
                {
                    if (directories.Contains(currentPath + lineArr[1]) == false)
                    {
                        directories.Add(currentPath + lineArr[1]);
                    }
                }
                //File
                else if (int.TryParse(lineArr[0], out _))
                {
                    if (files.Count(f => f.path == currentPath && f.filename == lineArr[1]) == 0)
                    {
                        files.Add(new FileObject(int.Parse(lineArr[0]), lineArr[1], currentPath));
                    }
                }
            }

            //Build list of directories with size
            List<DirectorySizeObject> dirWithSize = new List<DirectorySizeObject>();
            foreach (string dir in directories)
            {
                int dirSize = 0;
                foreach (FileObject file in files.Where(f => f.path.StartsWith(dir)))
                {
                    dirSize += file.filesize;
                }
                dirWithSize.Add(new DirectorySizeObject(dirSize, dir));
            }

            int totalDiskSpace = 70000000;
            int requiredFreeSpace = 30000000;
            int currentUsedSpace = dirWithSize.Find(d => d.path.Equals("/")).size;
            int minSizeToDelete = requiredFreeSpace - (totalDiskSpace - currentUsedSpace);

            return dirWithSize.Where(d => d.size >= minSizeToDelete).Min(d => d.size);
        }


        private class FileObject
        {
            public int filesize;
            public string filename;
            public string path;

            public FileObject(int filesize, string filename, string path)
            {
                this.filesize = filesize;
                this.filename = filename;
                this.path = path;
            }
            public FileObject()
            {
                this.filesize = 0;
                this.filename = "";
                this.path = "";
            }

            public string fullpath
            {
                get { return path + filename; }
                set { 
                    path = value.Substring(0, value.LastIndexOf('/'));
                    filename = value.Substring(value.LastIndexOf('/'));
                }
            }
        }

        private class DirectorySizeObject
        { 
            public int size;
            public string path;

            public DirectorySizeObject(int size, string path)
            {
                this.size = size;
                this.path = path;
            }
            public DirectorySizeObject()
            {
                this.size = 0;
                this.path = "";
            }
        }
    }
}
