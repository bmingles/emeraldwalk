using System;
namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public interface IFilePathService
    {
        string GetRemoteFilePath(string localFullFilePath);
        bool IsUnderLocalRoot(string localFullFilePath);
        string FixFilePathCasing(string filePath);
    }
}
