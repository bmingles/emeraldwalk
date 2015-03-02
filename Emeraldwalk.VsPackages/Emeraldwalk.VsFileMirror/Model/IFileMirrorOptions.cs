using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Commands;
using System.Collections.Generic;
namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Model
{
    public interface IFileMirrorOptions
    {
        bool IsConfigured { get; }

        string LocalRootPath { get; set; }        
        string RemoteFilePath { get; }
        string RemoteHost { get; set; }
        string RemoteRootPath { get; set; }
        string RemoteUsername { get; set; }
        char RemotePathSeparatorCharacter { get; set; }

        int CommandTimeout { get; set; }
        IList<CommandConfig> OnSaveCommands { get; }

        void OnPropertyChanged(string propertyName);
    }
}
