using Emeraldwalk.DirectoryWatch.Model;
using Emeraldwalk.DirectoryWatch.Services.Abstract;
using System.Collections.Generic;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Concrete
{
    public class CommandArgsService : ICommandArgsService
    {
        private string _getFullPath(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            return Path.GetFullPath(path);
        }

        public string BuildCommandArgs(
            FileSystemChangeType changeType,
            FileSystemObjectType fsoType,
            string watchDirPath,
            string changedFilePath,
            string originalFilePath,
            string filePathToProcess,
            string[] processExeArgs)
        {
            watchDirPath = this._getFullPath(watchDirPath ?? "");
            changedFilePath = this._getFullPath(changedFilePath ?? "");
            originalFilePath = this._getFullPath(originalFilePath ?? "");
            filePathToProcess = this._getFullPath(filePathToProcess ?? "");

            string argTokenStr = string.Concat(
                "\"",
                string.Join("\" \"", processExeArgs),
                "\"");

            IList<string> executableArgsList = new List<string>();

            string argsStr = argTokenStr.Replace(Token.ChangeType, changeType.ToString());
            argsStr = argsStr.Replace(Token.ObjectType, fsoType.ToString());

            argsStr = argsStr.Replace(Token.ChangedPath, changedFilePath);
            argsStr = argsStr.Replace(Token.ChangedPathNoExtension, Path.Combine(Path.GetDirectoryName(changedFilePath), Path.GetFileNameWithoutExtension(changedFilePath)));
            argsStr = argsStr.Replace(Token.ChangedPathRelative, changedFilePath.Substring(watchDirPath.Length + 1));

            argsStr = argsStr.Replace(Token.EachPath, filePathToProcess);
            argsStr = argsStr.Replace(Token.EachPathNoExtension, Path.Combine(Path.GetDirectoryName(filePathToProcess), Path.GetFileNameWithoutExtension(filePathToProcess)));
            argsStr = argsStr.Replace(Token.EachPathRelative, filePathToProcess.Substring(watchDirPath.Length + 1));

            argsStr = argsStr.Replace(Token.OriginalPath, originalFilePath);

            return argsStr;
        }
    }
}
