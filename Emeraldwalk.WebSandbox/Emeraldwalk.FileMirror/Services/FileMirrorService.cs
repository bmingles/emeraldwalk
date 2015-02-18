using Emeraldwalk.DirectoryWatch.Model;
using Emeraldwalk.DirectoryWatch.Services.Abstract;
using Emeraldwalk.DirectoryWatch.Services.Concrete;
using Emeraldwalk.FileMirror.Plugins.Infrastructure;
using Emeraldwalk.FileMirror.Plugins.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Emeraldwalk.FileMirror.Services
{
    public class FileMirrorService: DisposableBase
    {
        private readonly string _sourceFullRootPath;
        private readonly string _targetRootPath;
        private readonly IList<string> _filters;
        private readonly IList<IDirectoryWatcher> _directoryWatchers;
        private readonly IList<IFileMirrorPlugin> _plugins;
        private readonly string[] _pluginArgs;

        public FileMirrorService(
            string sourceFullRootPath,
            string targetRootPath,
            IList<string> filters,
            IList<IFileMirrorPlugin> plugins,
            params string[] pluginArgs)
        {
            this._sourceFullRootPath = sourceFullRootPath;
            this._targetRootPath = targetRootPath;
            this._filters = filters;
            this._plugins = plugins;
            this._pluginArgs = pluginArgs;

            this._directoryWatchers = new List<IDirectoryWatcher>();

            InitializePlugins();
        }

        public void StartWatchers()
        {
            foreach (string filter in this._filters)
            {
                IDirectoryWatcher watcher = new DirectoryWatcher();
                watcher.Changed += _directoryWatcher_Changed;

                this._directoryWatchers.Add(watcher);

                watcher.Start(this._sourceFullRootPath, filter);
            }
        }

        private void InitializePlugins()
        {
            foreach (IFileMirrorPlugin plugin in this._plugins.OrderBy(p => p.Priority))
            {
                plugin.Initialize(
                    this._sourceFullRootPath,
                    this._targetRootPath,
                    this._pluginArgs);
            }
        }        

        private void _directoryWatcher_Changed(object sender, FileSystemChangeEventArgs e)
        {
            foreach(IFileMirrorPlugin plugin in this._plugins.OrderBy(p => p.Priority))
            {
                if(e.FileSystemObjectType == FileSystemObjectType.File)
                {
                    HandleFileChange(plugin, e);
                }
                else if(e.FileSystemObjectType == FileSystemObjectType.Directory)
                {
                    HandleDirectoryChange(plugin, e);
                }
            }
        }

        private void HandleFileChange(
            IFileMirrorPlugin plugin,
            FileSystemChangeEventArgs e)
        {
            switch (e.ChangeType)
            {
                case FileSystemChangeType.Create:
                    plugin.CreateFile(e.FullPath);
                    break;

                case FileSystemChangeType.Change:
                    plugin.UpdateFile(e.FullPath);
                    break;

                case FileSystemChangeType.Rename:
                    plugin.RenameFile(e.OriginalFullPath, e.FullPath);
                    break;

                case FileSystemChangeType.Delete:
                    plugin.DeleteFile(e.FullPath);
                    break;

                default:
                    throw new NotSupportedException(string.Format("ChangeType '{0}' not supported.", e.ChangeType.ToString()));
            }
        }

        private void HandleDirectoryChange(
            IFileMirrorPlugin plugin,
            FileSystemChangeEventArgs e)
        {
            switch (e.ChangeType)
            {
                case FileSystemChangeType.Create:
                    plugin.CreateDirectory(e.FullPath);
                    break;

                case FileSystemChangeType.Change:
                    plugin.UpdateDirectory(e.FullPath);
                    break;

                case FileSystemChangeType.Rename:
                    plugin.RenameDirectory(e.OriginalFullPath, e.FullPath);
                    break;

                case FileSystemChangeType.Delete:
                    plugin.DeleteDirectory(e.FullPath);
                    break;

                default:
                    throw new NotSupportedException(string.Format("ChangeType '{0}' not supported.", e.ChangeType.ToString()));
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach(IFileMirrorPlugin plugin in this._plugins)
            {
                plugin.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
