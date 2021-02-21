using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileSearch
{
    class FileSearcher
    {
        private string rootDirectory = @"c:\";
        private string fileRegex = "*.*";
        public string currentDirectory;
        private uint foundFiles = 0;
        private uint allFoundFiles = 0;
        protected FileSystemNode root = new FileSystemNode();

        public FileSearcher()
        { }

        public FileSearcher SetRootDir(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
            this.currentDirectory = rootDirectory;
            return this;
        }

        public FileSearcher SetFilter(string fileRegex)
        {
            this.fileRegex = fileRegex;
            return this;
        }

        public void StarSearch()
        {
            SetParams();
            var thread = new Thread(() 
                => StartBuildFilesTreeOnNewBackgroundThread()) { IsBackground = true };
            thread.Start();
        }

        private void StartBuildFilesTreeOnNewBackgroundThread()
        {
            root.FullPath = rootDirectory;
            var foundFileSystemNodes = new Stack<FileSystemNode>();
            foundFileSystemNodes.Push(root);

            while (foundFileSystemNodes.Count > 0)
            {
                var currNode = foundFileSystemNodes.Pop();
                foreach (var currDir in Directory.GetDirectories(currNode.FullPath))
                {
                    var newNode = new FileSystemNode { FullPath = currDir };
                    currNode.Add(newNode);
                    foundFileSystemNodes.Push(newNode);
                }
                foreach (var currFile in Directory.GetFiles(currNode.FullPath))
                {
                    allFoundFiles++;
                    var currFileInfo = new FileInfo(currFile);
                    if (currFileInfo.Exists && Regex.IsMatch(currFileInfo.Name, Regex.Escape(fileRegex)))
                    {
                        foundFiles++;
                        var newNode = new FileSystemNode { FullPath = currFile, IsFile = true };
                        currNode.Add(newNode);
                    }
                }
            }
        }

        private void SetParams()
        {
            foundFiles = 0;
            allFoundFiles = 0;
        }
    }
}
