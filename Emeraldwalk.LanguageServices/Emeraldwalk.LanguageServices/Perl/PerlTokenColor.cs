
using Microsoft.VisualStudio.Package;
namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    /// <summary>
    /// Extending Microsoft.VisualStudio.Package.TokenColor
    /// </summary>
    //public enum PerlTokenColor
    //{
    //    Text = 0,
    //    Keyword = 1,
    //    Comment = 2,
    //    Identifier = 3,
    //    String = 4,
    //    Number = 5, // end default TokenColors
    //    Shebang = 6,
    //    Function = 7
    //}

    public struct PerlTokenColor
    {
        private readonly int _value;

        public PerlTokenColor(int value)
        {
            this._value = value;
        }

        public static readonly PerlTokenColor Text = 0;
        public static readonly PerlTokenColor Keyword = 1;
        public static readonly PerlTokenColor Comment = 2;
        public static readonly PerlTokenColor Identifier = 3;
        public static readonly PerlTokenColor String = 4;
        public static readonly PerlTokenColor Number = 5; // end default TokenColors
        public static readonly PerlTokenColor Shebang = 6;
        public static readonly PerlTokenColor Function = 7;

        public static implicit operator TokenColor(PerlTokenColor perlTokenColor)
        {
            return (TokenColor)perlTokenColor._value;
        }

        public static implicit operator PerlTokenColor(int value)
        {
            return new PerlTokenColor(value);
        }
    }
}
