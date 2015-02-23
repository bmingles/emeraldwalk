using System.ComponentModel;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands
{
    public class CommandConfig
    {
        [DisplayName(" Cmd")] //small display hack to force order
        public string Cmd { get; set; }
        public string Args { get; set; }

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

        public static CommandConfig Parse(string cmdStr)
        {
            cmdStr = cmdStr.Trim();
            int splitIndex = GetSplitIndex(cmdStr);

            if (splitIndex < cmdStr.Length)
            {
                return new CommandConfig
                {
                    Cmd = cmdStr.Substring(0, splitIndex),
                    Args = cmdStr.Substring(splitIndex + 1)
                };
            }
            else
            {
                return new CommandConfig
                {
                    Cmd = cmdStr,
                    Args = null
                };
            }
        }

        public override string ToString()
        {
            return Cmd;
        }
    }
}
