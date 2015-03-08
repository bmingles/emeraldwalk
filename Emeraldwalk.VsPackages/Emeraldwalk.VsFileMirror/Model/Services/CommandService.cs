using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services
{
    public class CommandService: ICommandService
    {
        private IConsole Console { get; set; }
        private IFileMirrorOptions Options { get; set; }
        private IFilePathService FilePathService { get; set; }

        public CommandService(
            IConsole console,
            IFileMirrorOptions options,
            IFilePathService filePathService)
        {
            this.Console = console;
            this.Options = options;
            this.FilePathService = filePathService;
        }

        public void RunOnSaveCommands(string fullLocalFilePath)
        {
            //ensure casing is correct for case sensitive commands
            fullLocalFilePath = this.FilePathService.FixFilePathCasing(fullLocalFilePath);

            Console.WriteLine("\r\n----------------------------------------------------------------------");
            Console.WriteLine(fullLocalFilePath);

            bool isUnderLocalRoot = this.FilePathService.IsUnderLocalRoot(fullLocalFilePath);

            int i = 0;
            foreach(CommandConfig cmdConfig in this.Options.OnSaveCommands.OrderBy(cmd => cmd.Priority))
            {
                ++i;                
                if (cmdConfig.IsEnabled)
                {
                    //not under local root
                    if(cmdConfig.RequireUnderRoot && !isUnderLocalRoot)
                    {
                        continue;
                    }

                    //filter mismatch
                    if (!string.IsNullOrWhiteSpace(cmdConfig.Filter) && !Regex.IsMatch(fullLocalFilePath, cmdConfig.Filter))
                    {
                        continue;
                    }

                    Console.Write("{0}{1}: ", i == 0 ? "" : "\r\n", i);

                    try
                    {
                        string args = CommandTokenService.ReplaceTokens(
                            cmdConfig.Args ?? "",
                            fullLocalFilePath,
                            this.FilePathService.GetRemoteFilePath(fullLocalFilePath),
                            this.Options);

                        new CommandRunner(
                            this.Console,
                            cmdConfig.Cmd,
                            args,
                            this.Options.CommandTimeout).Start();
                    }
                    catch(Exception e)
                    {
                        this.Console.WriteLine("{0}\r\n{1}", e.Message, e.StackTrace);
                    }
                }
            }
        }
    }
}
