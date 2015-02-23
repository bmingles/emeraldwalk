using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Views;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services;
using System.Threading;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror
{
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidEmeraldwalk_VsFileMirrorPkgString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideOptionPage(typeof(FileMirrorOptionPage), FileMirrorOptionPage.CATEGORY, FileMirrorOptionPage.PAGE_NAME, 0, 0, true)]
    public sealed class FileMirrorPackage : Package
    {
        public const string PACKAGE_DESCRIPTION = "Emeraldwalk - File Mirror";

        private Events DteEvents { get; set; }
        private DocumentEvents DocEvents { get; set; }
        private IConsole Console { get; set; }
        private FilePathService FilePathService { get; set; }
        private CommandService CommandService { get; set; }

        private IFileMirrorOptions _options;
        private IFileMirrorOptions Options
        {
            get { return this._options ?? (this._options = (FileMirrorOptionPage)this.GetDialogPage(typeof(FileMirrorOptionPage))); }
        }

        public FileMirrorPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            IVsOutputWindow outputWindow = this.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            this.Console = new VsPaneConsole(
                PACKAGE_DESCRIPTION,
                GuidList.guidEmeraldwalk_VsFileMirrorOutputPane,
                outputWindow);

            this.FilePathService = new FilePathService(this.Options);
            this.CommandService = new CommandService(this.Console, this.Options, this.FilePathService);

            DTE dte = this.GetService(typeof(DTE)) as DTE;
            this.DteEvents = dte.Events;
            this.DocEvents = this.DteEvents.DocumentEvents; //have to keep a reference to this to avoid GC
            this.DocEvents.DocumentSaved += DocumentEvents_DocumentSaved;

            // Queuing on the threadpool fixes an issue where console output doesn't show up in pane during initialization
            ThreadPool.QueueUserWorkItem(state =>
            {
                this.Console.WriteLine("Welcome to {0}", PACKAGE_DESCRIPTION);
                if (!this.Options.IsConfigured)
                {
                    this.Console.WriteLine("Incomplete configuration. Goto Tools -> Options -> Emeraldwalk to configure.");
                }
                this.Console.WriteOptions(this.Options);
            });
        }

        private void DocumentEvents_DocumentSaved(Document document)
        {
            if (!this.Options.IsConfigured)
            {
                this.Console.WriteLine("{0} not configured.", PACKAGE_DESCRIPTION);
                return;
            }

            ThreadPool.QueueUserWorkItem(state =>
            {
                this.CommandService.RunOnSaveCommands(document.FullName);
            });            
        }
    }
}
