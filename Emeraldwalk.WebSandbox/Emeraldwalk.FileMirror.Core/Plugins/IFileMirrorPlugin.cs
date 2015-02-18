using System;

namespace Emeraldwalk.FileMirror.Core.Plugins
{
    public interface IFileMirrorPlugin: IDisposable
    {
        int Priority { get; }

        void Initialize(
            string sourceFullRootPath,
            string targetRootPath,
            params string[] args);

        void CreateFile(string fullFilePath);
        void RenameFile(string origFullFilePath, string newFullFilePath);
        void UpdateFile(string fullFilePath);
        void DeleteFile(string fullFilePath);

        void CreateDirectory(string fullDirectoryPath);
        void RenameDirectory(string origFullDirPath, string newFullDirPath);
        void UpdateDirectory(string fullDirectoryPath);
        void DeleteDirectory(string fullDirectoryPath);
    }
}
