using Emeraldwalk.DirectoryWatch.Model;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Abstract
{
    public interface IDirectoryWatcher
    {
        event FileSystemChangeEventHandler Changed;
        void Start(string watchDirectory, string watchFilter);
    }
}
