using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using System.IO;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public static class CommandTokenService
    {
        public static string RemoveExtension(
            string path)
        {
            return path.Substring(0, path.Length - Path.GetExtension(path).Length);
        }

        public static string ReplaceTokens(
            string parameterizedCmd,
            string localFullFilePath,
            string remoteFullFilePath,
            IFileMirrorOptions options)
        {
            string cmd = parameterizedCmd
                .Replace(CommandTokens.LOCAL_ROOT, options.LocalRootPath)
                .Replace(CommandTokens.LOCAL_FILE, localFullFilePath)
                .Replace(CommandTokens.LOCAL_FILE_NOX, RemoveExtension(localFullFilePath))
                .Replace(CommandTokens.REMOTE_HOST, options.RemoteHost)
                .Replace(CommandTokens.REMOTE_USER, options.RemoteUsername)
                .Replace(CommandTokens.REMOTE_ROOT, options.RemoteRootPath)
                .Replace(CommandTokens.REMOTE_FILE, remoteFullFilePath)
                .Replace(CommandTokens.REMOTE_FILE_NOX, RemoveExtension(remoteFullFilePath));

            return cmd;
        }
    }
}
