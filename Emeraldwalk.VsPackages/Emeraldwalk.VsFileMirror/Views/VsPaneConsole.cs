using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Views
{
    public class VsPaneConsole : IConsole
    {
        private string PaneTitle { get; set; }
        private Guid PaneGuid { get; set; }
        private IVsOutputWindow OutputWindow { get; set; }
        private IVsOutputWindowPane OutputPane { get; set; }

        public VsPaneConsole(
            string paneTitle,
            Guid paneGuid,
            IVsOutputWindow outputWindow)
        {
            this.PaneTitle = paneTitle;
            this.PaneGuid = paneGuid;
            this.OutputWindow = outputWindow;

            this.InitializeOutputOutputPane();
        }

        private void InitializeOutputOutputPane()
        {
            Guid paneGuid = this.PaneGuid;
            this.OutputWindow.CreatePane(
                ref paneGuid,
                this.PaneTitle,
                1, 1);

            IVsOutputWindowPane pane;
            this.OutputWindow.GetPane(ref paneGuid, out pane);
            this.OutputPane = pane;
            this.OutputPane.Activate();
        }

        public void Write(string format, params object[] args)
        {
            this.OutputPane.Activate();
            this.OutputPane.OutputString(string.Format(format, args));
        }

        public void WriteLine(string format, params object[] args)
        {
            this.OutputPane.Activate();
            this.OutputPane.OutputString(
                string.Format("{0}{1}", 
                string.Format(format, args), 
                Environment.NewLine));
        }
        
        public void WriteOptions(IFileMirrorOptions options)
        {
            this.WriteLine("{0}: {1}", CommandTokens.LOCAL_ROOT, options.LocalRootPath);
            this.WriteLine("{0}: {1}", CommandTokens.REMOTE_HOST, options.RemoteHost);
            this.WriteLine("{0}: {1}", CommandTokens.REMOTE_USER, options.RemoteUsername);
            this.WriteLine("{0}: {1}", CommandTokens.REMOTE_ROOT, options.RemoteRootPath);
        }
    }
}
