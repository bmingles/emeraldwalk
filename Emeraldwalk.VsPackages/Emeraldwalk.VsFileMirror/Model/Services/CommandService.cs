using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;
using System;

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
            bool isUnderLocalRoot = this.FilePathService.IsUnderLocalRoot(fullLocalFilePath);

            int i = 0;
            foreach(CommandConfig cmdConfig in this.Options.OnSaveCommands)
            {
                Console.WriteLine("Command {0}:", ++i);
                if (cmdConfig.IsEnabled)
                {
                    if(cmdConfig.RequireUnderRoot && !isUnderLocalRoot)
                    {
                        this.Console.WriteLine("Local path '{0}' is not under local root path '{1}'.",
                            fullLocalFilePath, 
                            this.Options.LocalRootPath);
                        break;
                    }

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
                else
                {
                    this.Console.WriteLine("Disabled command.");
                }
            }
        }
    }
}
