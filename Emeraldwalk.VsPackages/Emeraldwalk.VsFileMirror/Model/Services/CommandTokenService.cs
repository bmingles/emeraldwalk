using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public static class CommandTokenService
    {
        public static string ReplaceTokens(
            string parameterizedCmd,
            string localFullFilePath,
            string remoteFullFilePath,
            IFileMirrorOptions options)
        {
            string cmd = parameterizedCmd
                .Replace(CommandTokens.LOCAL_ROOT, options.LocalRootPath)
                .Replace(CommandTokens.LOCAL_FILE, localFullFilePath)
                .Replace(CommandTokens.REMOTE_HOST, options.RemoteHost)
                .Replace(CommandTokens.REMOTE_USER, options.RemoteUsername)
                .Replace(CommandTokens.REMOTE_ROOT, options.RemoteRootPath)
                .Replace(CommandTokens.REMOTE_FILE, remoteFullFilePath);

            return cmd;
        }
    }
}
