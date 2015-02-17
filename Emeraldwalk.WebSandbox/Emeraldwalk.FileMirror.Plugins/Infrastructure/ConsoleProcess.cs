using System;
using System.Diagnostics;

namespace Emeraldwalk.FileMirror.Plugins.Infrastructure
{
    public class ConsoleProcess
    {
        public ConsoleProcess(
            string exePath,            
            string workingDir,
            params string[] exeArgs)
        {
            this.InitializeProcess(
                exePath,
                workingDir,
                string.Join(" ", exeArgs));
        }

        public Process Process { get; private set; }

        private void InitializeProcess(
            string exePath,
            string exeArgs,
            string workingDir)
        {
            Process process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(exePath)
                {
                    WorkingDirectory = workingDir,
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
                Console.WriteLine("'{0}' exited with code {1}", exePath, process.ExitCode);
            };

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                Console.WriteLine(e.Data);
            };

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                Console.WriteLine(e.Data);
            };

            this.Process = process;
        }

        public void Start()
        {
            this.Process.Start();
            this.Process.BeginOutputReadLine();
            this.Process.BeginErrorReadLine();
        }

        public void InputLine(string text)
        {
            this.Process.StandardInput.WriteLine(text);
        }
    }
}
