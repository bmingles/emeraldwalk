using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public class FilePathService : IFilePathService
    {
        private IFileMirrorOptions Options { get; set; }

        public FilePathService(IFileMirrorOptions options)
        {
            this.Options = options;
        }

        public bool IsUnderLocalRoot(string localFullFilePath)
        {
            if (localFullFilePath.Length < this.Options.LocalRootPath.Length)
            {
                return false;
            }

            return localFullFilePath.ToLower().StartsWith(this.Options.LocalRootPath.ToLower());
        }

        public string GetRemoteFilePath(
            string localFullFilePath)
        {
            if (!this.IsUnderLocalRoot(localFullFilePath))
            {
                return "[Local path must be under local root path.]";
            }

            string relativePath = localFullFilePath.Substring(this.Options.LocalRootPath.Length + 1);
            return Path.Combine(this.Options.RemoteRootPath, relativePath).Replace('\\', this.Options.RemotePathSeparatorCharacter);
        }

        /// <summary>
        /// For whatever reason sometimes filepaths may get changed from their
        /// actual casing in the filesystem. This helper can convert them
        /// to their actual casing for interacting with case sensitive tooling.
        /// </summary>
        public string FixFilePathCasing(string filePath)
        {
            string fullFilePath = Path.GetFullPath(filePath);

            IList<string> tokens = fullFilePath
                .Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            string fixedPath = null;

            //unc paths (NOTE: does not handle casing of shared folder directly under server)
            if (fullFilePath.StartsWith("\\\\"))
            {
                string serverPlusShareToken = string.Join("\\", tokens.Take(2));
                tokens.RemoveAt(0);
                tokens.RemoveAt(0);

                fixedPath = string.Concat("\\\\", serverPlusShareToken, "\\");
            }
            //local paths
            else
            {
                string driveToken = tokens[0];

                DriveInfo driveInfo = DriveInfo.GetDrives()
                    .First(di => di.Name.Equals(string.Concat(driveToken, "\\"), StringComparison.OrdinalIgnoreCase));

                driveToken = driveInfo.Name;
                tokens.RemoveAt(0);

                fixedPath = driveToken;
            }

            foreach (string token in tokens)
            {
                fixedPath = Directory.GetFileSystemEntries(fixedPath, token).First();
            }

            return fixedPath;
        }
    }
}
