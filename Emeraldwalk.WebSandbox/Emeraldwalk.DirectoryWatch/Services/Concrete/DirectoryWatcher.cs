using Emeraldwalk.DirectoryWatch.Model;
using Emeraldwalk.DirectoryWatch.Services.Abstract;
using System;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Concrete
{
    public class DirectoryWatcher : IDirectoryWatcher
    {
        public event FileSystemChangeEventHandler Changed;

        private FileSystemWatcher FileSystemWatcher { get; set; }

        public void Start(string watchDirectory, string watchFilter)
        {
            this.FileSystemWatcher = new FileSystemWatcher(watchDirectory, watchFilter);
            this.FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite|NotifyFilters.FileName|NotifyFilters.DirectoryName;
            this.FileSystemWatcher.EnableRaisingEvents = true;
            this.FileSystemWatcher.IncludeSubdirectories = true;
            this.FileSystemWatcher.Changed += FileSystemWatcher_Changed;
            this.FileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            this.FileSystemWatcher.Created += FileSystemWatcher_Created;
            this.FileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
        }

        private void _HandleChange(
            FileSystemChangeType changeType,
            string newPath,
            string origPath = null)
        {
            bool isDirectory = changeType == FileSystemChangeType.Delete
               ? !Path.HasExtension(newPath) // best we can do since object no longer exists
               : (File.GetAttributes(newPath) & FileAttributes.Directory) == FileAttributes.Directory;

            FileSystemObjectType fsoType = isDirectory
                ? FileSystemObjectType.Directory
                : FileSystemObjectType.File; ;

            Console.WriteLine(string.Concat(
                changeType.ToString(), 
                ": ", 
                newPath,
                origPath == null ? "" : ", orig: ",
                origPath == null ? "" : origPath,
                ", type: ",
                fsoType.ToString()));

            if (this.Changed != null)
            {
                //try
                //{
                    //this.FileSystemWatcher.EnableRaisingEvents = false;
                    this.Changed(this, new FileSystemChangeEventArgs
                    {
                        ChangeType = changeType,
                        FileSystemObjectType = fsoType,
                        FullPath = newPath,
                        OriginalFullPath = origPath
                    });
                //}
                //finally
                //{
                //    //this.FileSystemWatcher.EnableRaisingEvents = true;
                //}
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this._HandleChange(
                FileSystemChangeType.Create,
                e.FullPath);
        }        

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this._HandleChange(
                FileSystemChangeType.Change,
                e.FullPath);
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            this._HandleChange(
                FileSystemChangeType.Rename,
                e.FullPath, 
                e.OldFullPath);
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this._HandleChange(
                FileSystemChangeType.Delete,
                e.FullPath);
        }
    }
}
