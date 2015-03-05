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
            Console.WriteLine("\r\n----------------------------------------------------------------------");

            bool isUnderLocalRoot = this.FilePathService.IsUnderLocalRoot(fullLocalFilePath);

            int i = 0;
            foreach(CommandConfig cmdConfig in this.Options.OnSaveCommands.OrderBy(cmd => cmd.Priority))
            {
                Console.Write("{0}Cmd {1}: ", i == 0 ? "" : "\r\n", ++i);
                if (cmdConfig.IsEnabled)
                {
                    if(cmdConfig.RequireUnderRoot && !isUnderLocalRoot)
                    {
                        this.Console.WriteLine("Local path '{0}' is not under local root path '{1}'.",
                            fullLocalFilePath, 
                            this.Options.LocalRootPath);

                        continue; //move on to next cmd
                    }

                    try
                    {
                        if(!string.IsNullOrWhiteSpace(cmdConfig.Filter))
                        {
                            if(Regex.IsMatch(fullLocalFilePath, cmdConfig.Filter))
                            {
                                this.Console.WriteLine("Match '{0}'", cmdConfig.Filter);
                            }
                            else
                            {
                                this.Console.WriteLine("Skip '{0}'", cmdConfig.Filter);
                                continue; //move on to next cmd
                            }
                        }

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
                else
                {
                    this.Console.WriteLine("Disabled command.");
                }
            }
        }
    }
}
