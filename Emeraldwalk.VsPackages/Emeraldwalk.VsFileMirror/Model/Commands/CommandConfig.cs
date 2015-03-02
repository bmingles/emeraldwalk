using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands
{
    public class CommandConfig
    {
        public string Cmd { get; set; }
        public string Args { get; set; }
        public bool IsEnabled { get; set; }
        public bool RequireUnderRoot { get; set; }
        public string Filter { get; set; }
    }
}
