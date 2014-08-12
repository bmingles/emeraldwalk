using Emeraldwalk.DirectoryWatch.Services.Abstract;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Concrete
{
    public class DirectoryWatcher : IDirectoryWatcher
    {
        public event FileSystemEventHandler Changed;

        private FileSystemWatcher FileSystemWatcher { get; set; }

        public void Start(string watchDirectory, string watchFilter)
        {
            this.FileSystemWatcher = new FileSystemWatcher(watchDirectory, watchFilter);
            this.FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            this.FileSystemWatcher.EnableRaisingEvents = true;
            this.FileSystemWatcher.IncludeSubdirectories = true;
            this.FileSystemWatcher.Changed += FileSystemWatcher_Changed;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (this.Changed != null)
            {
                try
                {
                    this.FileSystemWatcher.EnableRaisingEvents = false;
                    this.Changed(this, e);
                }
                finally
                {
                    this.FileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }
    }
}
