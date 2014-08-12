using Emeraldwalk.DirectoryWatch.Services.Abstract;
using System.Collections.Generic;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Concrete
{
    public class CommandArgsService : ICommandArgsService
    {
        public string BuildCommandArgs(
            string changedFilePath,
            string filePathToProcess,
            string[] processExeArgs)
        {
            string argTokenStr = string.Concat(
                "\"",
                string.Join("\" \"", processExeArgs),
                "\"");

            IList<string> executableArgsList = new List<string>();

            string argsStr = argTokenStr.Replace(Token.ChangedFilePath, changedFilePath);
            argsStr = argsStr.Replace(Token.ChangedFilePathNoExtension, Path.Combine(Path.GetDirectoryName(changedFilePath), Path.GetFileNameWithoutExtension(changedFilePath)));

            argsStr = argsStr.Replace(Token.EachFilePath, filePathToProcess);
            argsStr = argsStr.Replace(Token.EachFilePathNoExtension, Path.Combine(Path.GetDirectoryName(filePathToProcess), Path.GetFileNameWithoutExtension(filePathToProcess)));

            return argsStr;
        }
    }
}
