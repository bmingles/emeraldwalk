using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands
{
    public class CommandConfig
    {
        public int? Priority { get; set; }
        public string Cmd { get; set; }
        public string Args { get; set; }
        public bool IsEnabled { get; set; }
        public bool RequireUnderRoot { get; set; }
        public string Filter { get; set; }

        public bool ShouldSave
        {
            get { 
                return this.Priority != null || 
                !string.IsNullOrWhiteSpace(this.Cmd) ||
                !string.IsNullOrWhiteSpace(this.Args) ||
                this.IsEnabled ||
                this.RequireUnderRoot ||
                !string.IsNullOrWhiteSpace(this.Filter); 
            }
        }
    }
}
