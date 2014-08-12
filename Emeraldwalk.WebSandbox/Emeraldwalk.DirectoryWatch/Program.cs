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

            config.WatchDirectory = q.Dequeue();
            config.Filter = q.Dequeue();
                        
            config.ProcessFileMode = ProcessFileMode.Changed;

            //optional mode: argument
            if(q.Peek().StartsWith("mode:"))
            {
                string processFileModeStr = q.Dequeue().Split(':')[1];
                config.ProcessFileMode = (ProcessFileMode)Enum.Parse(typeof(ProcessFileMode), processFileModeStr, true);
            }

            config.ExecutablePath = q.Dequeue();
            config.ExecutableArgs = q.ToArray();

            new DirectoryWatchService(
                config,
                new CommandArgsService(),
                new DirectoryWatcher())                
                .Start();

            Console.ReadLine();
        }
    }
}
