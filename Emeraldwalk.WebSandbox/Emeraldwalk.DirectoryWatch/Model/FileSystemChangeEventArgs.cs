using System;

namespace Emeraldwalk.DirectoryWatch.Model
{
    public class FileSystemChangeEventArgs: EventArgs
    {
        public FileSystemChangeType ChangeType { get; set; }
        public FileSystemObjectType FileSystemObjectType { get; set; }
        public string FullPath { get; set; }
        public string OriginalFullPath { get; set; }
    }
}
