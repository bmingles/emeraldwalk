// Guids.cs
// MUST match guids.h
using System;

namespace Emeraldwalk.PerlLanguage
{
    static class GuidList
    {
        public const string guidPerlLanguagePkgString = "7cff5ad0-6526-4cab-801c-da7c501d7831";
        public const string guidPerlLanguageCmdSetString = "e8dbd48d-dbc7-4591-9a3a-3e304b347274";

        public static readonly Guid guidPerlLanguageCmdSet = new Guid(guidPerlLanguageCmdSetString);
    };
}