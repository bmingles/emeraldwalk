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
                //throw new InvalidOperationException("Local path must be under local root path.");
            }

            string relativePath = localFullFilePath.Substring(this.Options.LocalRootPath.Length + 1);
            return Path.Combine(this.Options.RemoteRootPath, relativePath).Replace('\\', this.Options.RemotePathSeparatorCharacter);
        }
    }
}
