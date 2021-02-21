using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSearch
{
    class FileSystemNode : IEnumerable<FileSystemNode>
    {
        private List<FileSystemNode> nodes;

        public void Add(FileSystemNode node) => nodes.Add(node);

        public IEnumerable<FileSystemNode> FileSystemNodes
        {
            get
            {
                foreach (var node in nodes)
                    yield return node;
            }
        }
        public string Name { get => Path.GetFileName(FullPath); }

        public bool IsFile { get; set; }

        public string FullPath { get; set; }

        public FileSystemNode() => nodes = new List<FileSystemNode>();

        public bool HasFile
        {
            get => IsFile || nodes.Any(n => n.HasFile); 
        }

        public IEnumerator<FileSystemNode> GetEnumerator()
            => FileSystemNodes.Where(n => n.HasFile).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public override string ToString()
            => string.IsNullOrEmpty(Name) ? FullPath : Name;
    }
}
