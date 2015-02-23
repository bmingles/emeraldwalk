using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Views
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class FileMirrorOptionPage : DialogPage, IFileMirrorOptions
    {
        public const string CATEGORY = "Emeraldwalk";
        public const string PAGE_NAME = "File Mirror";

        private UserControl _view;

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

        private string ConfiguredCommandSummary
        {
            get
            {
                return CommandTokenService.ReplaceTokens(
                    this.OnSaveCommandsEditor,
                    string.Format(@"{0}\{1}", this.LocalRootPath, this.MockRelativeLocalFilePath),
                    string.Format("{0}{1}{2}", this.RemoteRootPath, this.RemotePathSeparatorCharacter, this.MockRelativeRemoteFilePath),
                    this);
            }
        }

        /// <summary>
        /// Wrapping default property grid so that we can add a convenience textbox to display
        /// configured commands a little clearer.
        /// </summary>
        protected override System.Windows.Forms.IWin32Window Window
        {
            get
            {
                if (_view == null)
                {
                    this._view = new UserControl();

                    //default implementation PropertyGrid
                    PropertyGrid propertyGrid = base.Window as PropertyGrid;
                    propertyGrid.Dock = DockStyle.Fill;
                    propertyGrid.PropertySort = PropertySort.Categorized;
                    this._view.Controls.Add(propertyGrid);

                    TextBox textBox = new TextBox
                    {
                        Text = this.ConfiguredCommandSummary,
                        Multiline = true,
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        ScrollBars = ScrollBars.Vertical
                    };

                    GroupBox groupBox = new GroupBox
                    {
                        Text = "Configured Save Commands",
                        Dock = DockStyle.Bottom,
                        Height = 80,
                    };

                    propertyGrid.PropertyValueChanged += (object s, PropertyValueChangedEventArgs e) =>
                    {
                        textBox.Text = this.ConfiguredCommandSummary;
                        propertyGrid.Refresh();
                    };

                    groupBox.Controls.Add(textBox);
                    this._view.Controls.Add(groupBox);
                }

                return this._view;
            }
        }

        #region Hidden From Editor

        [Browsable(false)]
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

        private IList<CommandConfig> _onSaveCommands;
        [Browsable(false)]
        public IList<CommandConfig> OnSaveCommands
        {
            get { return this._onSaveCommands ?? (this._onSaveCommands = new List<CommandConfig>()); }
        }

        #endregion

        #region Editor

        private int? _commandTimeout;

        [Category("Command Settings")]
        [DisplayName("Command Timeout (Seconds)")]
        [Description("Command Timeout in Seconds")]
        public int CommandTimeout
        {
            get { return this._commandTimeout ?? 10; }
            set { this._commandTimeout = value; }
        }

        [Category("Local Config")]
        [DisplayName(CommandTokens.LOCAL_FILE)]
        [Description("Local file path")]
        public string LocalFilePath
        {
            get { return Path.Combine(this.LocalRootPath, this.MockRelativeLocalFilePath); }
        }

        [Category("Local Config")]
        [DisplayName(CommandTokens.LOCAL_ROOT)]
        [Description("Local root path")]
        public string LocalRootPath { get; set; }

        [Category("Remote Server")]
        [DisplayName(CommandTokens.REMOTE_HOST)]
        [Description("Remote host address")]
        public string RemoteHost { get; set; }

        [Category("Remote Server")]
        [DisplayName(CommandTokens.REMOTE_USER)]
        [Description("Remote server username")]
        public string RemoteUsername { get; set; }

        [Category("Remote Server")]
        [DisplayName(CommandTokens.REMOTE_ROOT)]
        [Description("Remote root path")]
        public string RemoteRootPath { get; set; }

        private char? _remotePathSeparatorCharacter;

        [Category("Remote Server")]
        [DisplayName("Remote Path Separator")]
        [Description("Remote path separator")]
        public char RemotePathSeparatorCharacter
        {
            get { return this._remotePathSeparatorCharacter ?? '/'; }
            set { this._remotePathSeparatorCharacter = value; }
        }

        [Category("Remote Server")]
        [DisplayName(CommandTokens.REMOTE_FILE)]
        [Description("Remote file path. Combination of remote root + relative path of file being saved")]
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

        [Category("Save Commands")]
        [DisplayName("Commands (1 per line)")]
        [Description("List of commands to execute on save")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string OnSaveCommandsEditor
        {
            get
            {
                return string.Join("\r\n", this.OnSaveCommands.Select(cmd => string.Format("{0} {1}", cmd.Cmd, cmd.Args)));
            }
            set
            {
                this.OnSaveCommands.Clear();
                foreach (string cmd in value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    this.OnSaveCommands.Add(CommandConfig.Parse(cmd));
                }
            }
        }

        #endregion
    }
}
