using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.DirectoryWatch
{
    public static class Token
    {
        public const string ChangeType = "{ChangeType}";
        public const string ObjectType = "{ObjectType}";

        public const string ChangedPath = "{Changed}";
        public const string ChangedPathNoExtension = "{ChangedNoX}";
        public const string ChangedPathRelative = "{ChangedRel}";

        public const string OriginalPath = "{Original}";

        public const string EachPath = "{Each}";
        public const string EachPathNoExtension = "{EachNoX}";
        public const string EachPathRelative = "{EachRel}";
    }
}
