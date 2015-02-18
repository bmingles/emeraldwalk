using Emeraldwalk.FileMirror.Core.Plugins;
using Emeraldwalk.FileMirror.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Emeraldwalk.FileMirror
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> q = new Queue<string>(args);

            if(q.Count < 3)
            {
                Console.WriteLine("Invalid args. Expected:\r\nwatchdir filter destdir [arg1 [arg2 [argn]]");
                Console.Read();
                return;
            }

            string watchDirectoryFullPath = Path.GetFullPath(q.Dequeue());
            string watchFilter = q.Dequeue();
            string destinationRootPath = q.Dequeue();

            Console.Title = string.Join(" ", args);
            Console.WriteLine(Console.Title);
            
            string pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            pluginDir = Path.Combine(pluginDir, "Plugins");
            PluginService pluginService = new PluginService(pluginDir);
            IList<IFileMirrorPlugin> plugins = pluginService.LoadPlugins();

            using (FileMirrorService fileMirrorService = new FileMirrorService(
                watchDirectoryFullPath,
                destinationRootPath,
                watchFilter.Split('|'),
                plugins,
                q.ToArray()))
            {
                //start watching directory
                fileMirrorService.StartWatchers();

                Console.WriteLine("Supported commands: exit, cls.");

                //loop until user exit
                string line;
                while ((line = Console.ReadLine().ToLower()) != "exit")
                {
                    if (line == "cls")
                    {
                        Console.Clear();
                    }
                }
            }
        }
    }
}
