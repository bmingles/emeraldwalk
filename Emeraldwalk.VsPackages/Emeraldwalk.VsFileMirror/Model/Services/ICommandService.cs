
namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public interface ICommandService
    {
        void RunOnSaveCommands(string fullLocalFilePath);
    }
}
