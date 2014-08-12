namespace Emeraldwalk.DirectoryWatch.Services.Abstract
{
    public interface ICommandArgsService
    {
        string BuildCommandArgs(            
            string changedFilePath,
            string filePathToProcess,
            string[] processExeArgs);
    }
}
