using Emeraldwalk.DirectoryWatch.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Emeraldwalk.DirectoryWatch.Services.Concrete
{
    public class DirectoryWatchService
    {
        private IDirectoryWatcher DirectoryWatcher;
        private ICommandArgsService ProcessFileCommandArgsService { get; set; }
        public DirectoryWatchConfig Config { get; private set; }

        public DirectoryWatchService(
            DirectoryWatchConfig config,
            ICommandArgsService processFileCommandArgsService,
            IDirectoryWatcher directoryWatcher)
        {
            this.Config = config;
            this.ProcessFileCommandArgsService = processFileCommandArgsService;

            this.DirectoryWatcher = directoryWatcher;
            this.DirectoryWatcher.Changed += _fileSystemWatcher_Changed;
        }

        private void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath + " changed.");

            IList<string> filesToProcess = this.GetFilesToProcess(e.FullPath);

            foreach (string filePathToProcess in filesToProcess)
            {
                string argStr = this.ProcessFileCommandArgsService.BuildCommandArgs(
                    e.FullPath,
                    filePathToProcess,
                    this.Config.ExecutableArgs);

                RunProcess(argStr);
            }
        }

        private void RunProcess(string argsStr)
        {
            Console.WriteLine("Executing: \"{0}\" {1}", this.Config.ExecutablePath, argsStr);

            ProcessStartInfo tsProcessInfo = new ProcessStartInfo(this.Config.ExecutablePath)
            {
                WorkingDirectory = this.Config.WatchDirectory,
                Arguments = argsStr,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process process = Process.Start(tsProcessInfo);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine(error ?? output);
            Console.WriteLine("done.");
        }

        public void Start()
        {
            Console.WriteLine(string.Format(
                "Starting watch on \"{0}\", filter:{1}, mode:{2}",
                this.Config.WatchDirectory,
                this.Config.Filter,
                this.Config.ProcessFileMode));

            if (!Directory.Exists(this.Config.WatchDirectory))
            {
                throw new DirectoryNotFoundException(this.Config.WatchDirectory);
            }

            this.DirectoryWatcher.Start(this.Config.WatchDirectory, this.Config.Filter);
        }

        public IList<string> GetFilesToProcess(
            string changedFilePath)
        {
            string[] filePaths;

            switch (this.Config.ProcessFileMode)
            {
                case ProcessFileMode.All:
                    filePaths = Directory.GetFiles(this.Config.WatchDirectory, "*.*", SearchOption.AllDirectories);
                    break;
                case ProcessFileMode.Filtered:
                    filePaths = Directory.GetFiles(this.Config.WatchDirectory, this.Config.Filter, SearchOption.AllDirectories);
                    break;

                case ProcessFileMode.Changed:
                default:
                    filePaths = new[] { changedFilePath };
                    break;
            }

            return filePaths;
        }
    }
}
