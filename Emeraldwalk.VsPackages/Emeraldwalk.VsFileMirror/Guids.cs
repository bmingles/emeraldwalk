// Guids.cs
// MUST match guids.h
using System;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror
{
    static class GuidList
    {
        public const string guidEmeraldwalk_VsFileMirrorPkgString = "396fd2dd-3a1d-47fa-8971-5eb8606b9bac";
        public const string guidEmeraldwalk_VsFileMirrorCmdSetString = "ac279a89-1cc6-41ab-a3cc-d8300dabfc11";
        public const string guidEmeraldwalk_VsFileMirrorOutputPaneString = "FF2C6FC0-F8CD-4266-821B-FB89F473E0F1";

        public static readonly Guid guidEmeraldwalk_VsFileMirrorCmdSet = new Guid(guidEmeraldwalk_VsFileMirrorCmdSetString);
        public static readonly Guid guidEmeraldwalk_VsFileMirrorOutputPane = new Guid(guidEmeraldwalk_VsFileMirrorOutputPaneString);
    };
}