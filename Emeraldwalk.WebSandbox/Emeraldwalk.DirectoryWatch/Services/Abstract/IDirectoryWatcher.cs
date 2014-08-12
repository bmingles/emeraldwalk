using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Abstract
{
    public interface IDirectoryWatcher
    {
        event FileSystemEventHandler Changed;
        void Start(string watchDirectory, string watchFilter);
    }
}
