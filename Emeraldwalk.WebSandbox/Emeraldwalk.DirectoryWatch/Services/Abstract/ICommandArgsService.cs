using Emeraldwalk.DirectoryWatch.Model;
namespace Emeraldwalk.DirectoryWatch.Services.Abstract
{
    public interface ICommandArgsService
    {
        string BuildCommandArgs(
            FileSystemChangeType changeType,
            FileSystemObjectType fsoType,
            string watchDirPath,
            string changedFilePath,
            string originalFilePath,
            string filePathToProcess,
            string[] processExeArgs);
    }
}
