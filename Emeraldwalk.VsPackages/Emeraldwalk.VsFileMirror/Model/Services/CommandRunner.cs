using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;
using System;
using System.Diagnostics;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public class CommandRunner
    {
        private Process Process { get; set; }
        private IConsole Console { get; set; }
        private int TimeoutSeconds { get; set; }

        public CommandRunner(
            IConsole console,
            string cmd,
            string args,
            int timeoutSeconds)
        {
            this.Console = console;
            this.InitializeProcess(cmd, args);
            this.TimeoutSeconds = timeoutSeconds;
        }

        private void InitializeProcess(
            string exePath,
            //string workingDir,
            string exeArgs)
        {
            Process process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(exePath)
                {
                    //WorkingDirectory = workingDir,
                    Arguments = exeArgs,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Exited += (object sender, EventArgs e) =>
            {
                this.Console.WriteLine("'{0}' exited with code {1}", exePath, process.ExitCode);
            };

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    this.Console.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    this.Console.WriteLine(e.Data);
                }
            };

            this.Process = process;
        }

        public void Start()
        {
            this.Console.WriteLine("{0} {1}", this.Process.StartInfo.FileName, this.Process.StartInfo.Arguments);
            this.Process.Start();
            this.Process.BeginOutputReadLine();
            this.Process.BeginErrorReadLine();

            if (!this.Process.WaitForExit(this.TimeoutSeconds * 1000))
            {
                this.Process.Kill();
            }
        }

        public void InputLine(string text)
        {
            this.Process.StandardInput.WriteLine(text);
        }
    }
}
