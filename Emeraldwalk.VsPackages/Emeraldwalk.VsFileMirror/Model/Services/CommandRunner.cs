using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;
using System;
using System.Diagnostics;
using System.Threading;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public class CommandRunner
    {
        private Process Process { get; set; }
        private IConsole Console { get; set; }
        private int TimeoutSeconds { get; set; }
        private CountdownEvent CountdownEvent { get; set; }

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

        private void HandleData(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                this.CountdownEvent.Signal(); //decrement countdown
            }
            else if (!string.IsNullOrWhiteSpace(e.Data))
            {
                this.Console.WriteLine(e.Data);
            }
        }

        private void InitializeProcess(
            string exePath,
            string exeArgs)
        {
            Process process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(exePath)
                {
                    Arguments = exeArgs,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.OutputDataReceived += this.HandleData;
            process.ErrorDataReceived += this.HandleData;

            process.Exited += (object sender, EventArgs e) =>
            {
                this.Console.WriteLine("'{0}' exited with code {1}", exePath, process.ExitCode);
                this.CountdownEvent.Signal(); //decrement countdown
            };

            this.Process = process;
        }

        public void Start()
        {
            this.CountdownEvent = new CountdownEvent(3); //Count standard and error output and exited event completion

            this.Console.WriteLine("{0} {1}", this.Process.StartInfo.FileName, this.Process.StartInfo.Arguments);
            this.Process.Start();
            this.Process.BeginOutputReadLine();
            this.Process.BeginErrorReadLine();

            if (!this.Process.WaitForExit(this.TimeoutSeconds * 1000))
            {
                this.Process.Kill();                
                this.CountdownEvent.Wait();
            }
        }

        public void InputLine(string text)
        {
            this.Process.StandardInput.WriteLine(text);
        }
    }
}
