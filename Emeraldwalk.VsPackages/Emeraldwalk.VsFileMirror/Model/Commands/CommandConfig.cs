using System.ComponentModel;
using System.Linq;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands
{
    public class CommandConfig
    {
        [DisplayName(" Cmd")] //small display hack to force order
        public string Cmd { get; set; }
        public string Args { get; set; }
        public bool IsEnabled { get; set; }
        public bool RequireUnderRoot { get; set; }

        private static int GetSplitIndex(string cmdStr)
        {
            bool inQuotes = false;
            int i = 0;
            for (i = 0; i < cmdStr.Length; ++i)
            {
                char c = cmdStr[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }

                if (!inQuotes && c == ' ')
                {
                    return i;
                }
            }

            return i;
        }

        /// <summary>
        /// Parse a string representation of a cmd
        /// Format: IsEnabled RequireUnderRoot cmd args
        /// </summary>
        public static CommandConfig Parse(string cmdStr)
        {
            cmdStr = cmdStr.Trim();
            string[] split = cmdStr.Split(' ');

            bool isEnabled = bool.Parse(split[0]);
            bool requireUnderRoot = bool.Parse(split[1]);
            cmdStr = string.Join(" ", split.Skip(2));

            CommandConfig config = new CommandConfig
            {
                IsEnabled = isEnabled,
                RequireUnderRoot = requireUnderRoot
            };            
            
            int splitIndex = GetSplitIndex(cmdStr);

            if (splitIndex < cmdStr.Length)
            {
                config.Cmd = cmdStr.Substring(0, splitIndex);
                config.Args = cmdStr.Substring(splitIndex + 1);
            }
            else
            {
                config.Cmd = cmdStr;
            }

            return config;
        }

        public override string ToString()
        {
            return Cmd;
        }
    }
}
