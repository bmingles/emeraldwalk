using Emeraldwalk.FileMirror.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Emeraldwalk.FileMirror.Services
{
    public class PluginService
    {
        private readonly string _pluginDirectoryPath;

        public PluginService(string pluginDirectoryPath)
        {
            this._pluginDirectoryPath = pluginDirectoryPath;
        }

        public IList<IFileMirrorPlugin> LoadPlugins()
        {
            IList<IFileMirrorPlugin> plugins = new List<IFileMirrorPlugin>();

            foreach (string dllPath in Directory.GetFiles(
                this._pluginDirectoryPath, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(dllPath);
                foreach(Type pluginType in assembly.GetTypes().Where(type => type.GetInterface("IFileMirrorPlugin") != null))
                {
                    plugins.Add((IFileMirrorPlugin)Activator.CreateInstance(pluginType));
                }
            }

            return plugins;
        }
    }
}
