using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Views
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("47573927-FB20-4BC7-979A-E37960F0F6C3")]
    public class FileMirrorOptions : UIElementDialogPage, IFileMirrorOptions, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public const string CATEGORY = "Emeraldwalk";
        public const string PAGE_NAME = "File Mirror";

        protected override System.Windows.UIElement Child
        {
            get { return new FileMirrorOptionsControl(this); }
        }

        private string MockRelativeFilePathFormat
        {
            get { return "somedir{0}file-being-saved.txt"; }
        }

        private string MockRelativeLocalFilePath
        {
            get
            {
                return string.Format(
                    this.MockRelativeFilePathFormat,
                    @"\");
            }
        }

        private string MockRelativeRemoteFilePath
        {
            get
            {
                return string.Format(
                    this.MockRelativeFilePathFormat,
                    this.RemotePathSeparatorCharacter);
            }
        }

        public bool IsConfigured
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(this.LocalRootPath) &&
                    !string.IsNullOrWhiteSpace(this.RemoteHost) &&
                    !string.IsNullOrWhiteSpace(this.RemoteRootPath) &&
                    !string.IsNullOrWhiteSpace(this.RemoteUsername);
            }
        }

        public string LocalFilePath
        {
            get { return Path.Combine(this.LocalRootPath, this.MockRelativeLocalFilePath); }
        }

        public string RemoteFilePath
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    CommandTokens.REMOTE_ROOT,
                    this.RemotePathSeparatorCharacter,
                    this.MockRelativeRemoteFilePath);
            }
        }

        private string _localRootPath;
        public string LocalRootPath
        {
            get { return this._localRootPath; }
            set { 
                this._localRootPath = value;
                this.OnPropertyChanged("LocalRootPath");
            }
        }

        private string _remoteHost;
        public string RemoteHost
        {
            get { return this._remoteHost; }
            set
            {
                this._remoteHost = value;
                this.OnPropertyChanged("RemoteHost");
            }
        }

        private string _remoteRootPath;
        public string RemoteRootPath
        {
            get { return this._remoteRootPath; }
            set
            {
                this._remoteRootPath = value;
                this.OnPropertyChanged("RemoteRootPath");
            }
        }

        private string _remoteUserName;
        public string RemoteUsername
        {
            get { return this._remoteUserName; }
            set
            {
                this._remoteUserName = value;
                this.OnPropertyChanged("RemoteUsername");
            }
        }
        
        private char? _remotePathSeparatorCharacter;
        public char RemotePathSeparatorCharacter
        {
            get { return this._remotePathSeparatorCharacter ?? '/'; }
            set 
            {
                this._remotePathSeparatorCharacter = value;
                this.OnPropertyChanged("RemotePathSeparatorCharacter");
                this.OnPropertyChanged("RemoteFilePath");
            }
        }

        private int? _commandTimeout;
        public int CommandTimeout
        {
            get { return this._commandTimeout ?? 10; }
            set 
            { 
                this._commandTimeout = value;
                this.OnPropertyChanged("CommandTimeout");
            }
        }
        
        private IList<CommandConfig> _onSaveCommands;        
        public IList<CommandConfig> OnSaveCommands
        {
            get { return this._onSaveCommands ?? (this._onSaveCommands = new ObservableCollection<CommandConfig>()); }
        }

        /// <summary>
        /// Poor man's persistence mechanism since string values will be auto-persisted.
        /// </summary>
        public string OnSaveCommandsEditor
        {
            get
            {
                return string.Join("\r\n", this.OnSaveCommands.Select(cmd => string.Format("{0} {1} {2} {3}", cmd.IsEnabled, cmd.RequireUnderRoot, cmd.Cmd, cmd.Args)));
            }
            set
            {
                this.OnSaveCommands.Clear();
                foreach (string cmd in value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    try
                    {
                        this.OnSaveCommands.Add(CommandConfig.Parse(cmd));
                    }
                    catch{}
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
