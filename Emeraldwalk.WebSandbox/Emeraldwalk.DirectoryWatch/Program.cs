using Emeraldwalk.DirectoryWatch.Services.Concrete;
using System;
using System.Collections.Generic;

namespace Emeraldwalk.DirectoryWatch
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryWatchConfig config = new DirectoryWatchConfig();

            Queue<string> q = new Queue<string>(args);

            if(q.Count < 3)
            {
                Console.WriteLine("Invalid args. Expected:\r\nwatchdir filter [mode:Changed|Filtered|All] exepath [[[exearg1], exearg2], exeargn]");
                Console.Read();
                return;
            }

            config.WatchDirectory = q.Dequeue();
            config.Filter = q.Dequeue();
                       
            //optional mode: argument
            config.ProcessFileMode = ProcessFileMode.Changed;
            if(q.Peek().StartsWith("mode:"))
            {
                string processFileModeStr = q.Dequeue().Split(':')[1];
                config.ProcessFileMode = (ProcessFileMode)Enum.Parse(typeof(ProcessFileMode), processFileModeStr, true);
            }

            config.ExecutablePath = q.Dequeue();
            config.ExecutableArgs = q.ToArray();

            Console.Title = config.ExecutablePath;
            
            new DirectoryWatchService(
                config,
                new CommandArgsService(),
                new DirectoryWatcher())                
                .Start();

            Console.WriteLine("Supported commands: exit, cls.");

            string line;
            while ((line = Console.ReadLine().ToLower()) != "exit")
            {
                if(line == "cls")
                {
                    Console.Clear();
                }
            }
        }
    }
}
