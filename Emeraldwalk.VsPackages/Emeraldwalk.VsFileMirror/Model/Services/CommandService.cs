using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;

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
            foreach(CommandConfig cmdConfig in this.Options.OnSaveCommands)
            {
                string args = CommandTokenService.ReplaceTokens(
                    cmdConfig.Args, 
                    fullLocalFilePath,
                    this.FilePathService.GetRemoteFilePath(fullLocalFilePath),
                    this.Options);

                new CommandRunner(
                    this.Console,
                    cmdConfig.Cmd,
                    args,
                    this.Options.CommandTimeout).Start();
            }
        }
    }
}
