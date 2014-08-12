using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.DirectoryWatch
{
    public class DirectoryWatchConfig
    {
        public string WatchDirectory { get; set; }
        public string Filter { get; set; }
        public ProcessFileMode ProcessFileMode { get; set; }
        public string ExecutablePath { get; set; }
        public string[] ExecutableArgs { get; set; }
    }
}
