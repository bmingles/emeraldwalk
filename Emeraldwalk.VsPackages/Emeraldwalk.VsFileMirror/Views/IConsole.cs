
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Views
{
    public interface IConsole
    {
        void Write(string format, params object[] args);
        void WriteLine(string format, params object[] args);
        void WriteOptions(IFileMirrorOptions options);
    }
}
