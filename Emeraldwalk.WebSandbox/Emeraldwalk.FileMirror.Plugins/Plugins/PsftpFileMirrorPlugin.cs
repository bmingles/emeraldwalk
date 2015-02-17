using Emeraldwalk.FileMirror.Plugins.Infrastructure;
using System;
using System.IO;

namespace Emeraldwalk.FileMirror.Plugins.Plugins
{
    public class PsftpFileMirrorPlugin: IFileMirrorPlugin
    {
        private ConsoleProcess _psftpProcess;
        //private Process _plinkProcess;
        private string _userHost;
        private string _sourceFullRootPath;
        private string _targetRootPath;

        public int Priority
        {
            get { return 5; }
        }

        private string DestinationPathSeparator
        {
            get { return "/"; }
        }

        public void Initialize(
            string sourceFullRootPath,
            string targetRootPath,
            params string[] args)
        {
            Console.Write("Initializing PsftpFileMirrorPlugin: ");
            Console.WriteLine(string.Join(" ", args));

            this._sourceFullRootPath = sourceFullRootPath;
            this._targetRootPath = targetRootPath;
            this._userHost = args[0];

            this._psftpProcess = new ConsoleProcess(
                @"C:\Program Files (x86)\PuTTY\psftp.exe",
                this._sourceFullRootPath,
                this._userHost);

            Console.WriteLine("psftp {0}", this._userHost);
            this._psftpProcess.Start();

            this._psftpProcess.InputLine(string.Format("cd \"{0}\"", this._targetRootPath));

            //this._plinkProcess = this.StartProcess(@"C:\Program Files (x86)\PuTTY\plink.exe");
        }

        private string GetDestinationPath(string sourceFullPath)
        {
            string relativePath = sourceFullPath.Substring(this._sourceFullRootPath.Length + 1);
            string destinationPath = Path.Combine(this._targetRootPath, relativePath);
            return destinationPath.Replace("\\", this.DestinationPathSeparator);
        }

        private void RenameFileSystemObject(string origFullFilePath, string newFullFilePath)
        {
            string origDestination = this.GetDestinationPath(origFullFilePath);
            string newDestination = this.GetDestinationPath(newFullFilePath);

            Console.WriteLine(string.Concat("Renaming: ", origDestination, " to ", newDestination));

            this._psftpProcess.InputLine(string.Format("rename \"{0}\" \"{1}\"",
                origDestination,
                newDestination));
        }

        private void ExecuteSinglePathPsftpCmd(
            string cmd,
            string description,
            string fullPath)
        {
            string destinationPath = this.GetDestinationPath(fullPath);

            Console.WriteLine(string.Concat(description, ": ", destinationPath));

            this._psftpProcess.InputLine(string.Format("{0} \"{1}\" \"{2}\"",
                cmd,
                fullPath,
                destinationPath));
        }

        public void CreateFile(string fullFilePath)
        {
            this.ExecuteSinglePathPsftpCmd("put", "Creating", fullFilePath);
        }
               
        public void RenameFile(string origFullFilePath, string newFullFilePath)
        {
            this.RenameFileSystemObject(origFullFilePath, newFullFilePath);
        }

        public void UpdateFile(string fullFilePath)
        {
            this.ExecuteSinglePathPsftpCmd("put", "Updating", fullFilePath);
        }

        public void DeleteFile(string fullFilePath)
        {
            this.ExecuteSinglePathPsftpCmd("rm", "Deleting", fullFilePath);
        }

        public void CreateDirectory(string fullDirectoryPath)
        {
            this.ExecuteSinglePathPsftpCmd("mkdir", "Creating Dir", fullDirectoryPath);
        }

        public void RenameDirectory(string origFullDirPath, string newFullDirPath)
        {
            this.RenameFileSystemObject(origFullDirPath, newFullDirPath);
        }

        public void UpdateDirectory(string fullDirectoryPath)
        {
        }

        public void DeleteDirectory(string fullDirectoryPath)
        {
            this.ExecuteSinglePathPsftpCmd("rmdir", "Deleting Dir", fullDirectoryPath);
            //string destinationPath = this.GetDestinationPath(fullDirectoryPath);
            //this._plinkProcess.StandardInput.WriteLine(string.Format("rd -r \"{0}\"", destinationPath));
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing PsftpFileMirrorPlugin");
            this._psftpProcess.InputLine("exit");
            //this._plinkProcess.StandardInput.WriteLine("exit");
        }
    }
}
