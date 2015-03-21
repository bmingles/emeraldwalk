
using Microsoft.VisualStudio.Package;
using System;
namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    /// <summary>
    /// Extending Microsoft.VisualStudio.Package.TokenType
    /// </summary>
    //public enum PerlTokenType
    //{
    //    Unknown = 0,
    //    Text = 1,
    //    Keyword = 2,
    //    Identifier = 3,
    //    String = 4,
    //    Literal = 5,
    //    Operator = 6,
    //    Delimiter = 7,
    //    WhiteSpace = 8,
    //    LineComment = 9,
    //    Comment = 10, //end default TokenTypes
    //    Shebang = 11,
    //    Function = 12
    //}

    public struct PerlTokenType
    {
        private readonly int _value;

        public PerlTokenType(int value)
        {
            this._value = value;
        }        

        public static readonly PerlTokenType Unknown = 0;
        public static readonly PerlTokenType Text = 1;
        public static readonly PerlTokenType Keyword = 2;
        public static readonly PerlTokenType Identifier = 3;
        public static readonly PerlTokenType String = 4;
        public static readonly PerlTokenType Literal = 5;
        public static readonly PerlTokenType Operator = 6;
        public static readonly PerlTokenType Delimiter = 7;
        public static readonly PerlTokenType WhiteSpace = 8;
        public static readonly PerlTokenType LineComment = 9;
        public static readonly PerlTokenType Comment = 10; //end default TokenTypes
        public static readonly PerlTokenType Shebang = 11;
        public static readonly PerlTokenType Function = 12;
        public static readonly PerlTokenType CurlyBrace = 13;
        public static readonly PerlTokenType SquareBrace = 14;
        public static readonly PerlTokenType Parentheses = 15;

        public static implicit operator TokenType(PerlTokenType perlTokenType)
        {
            return (TokenType)perlTokenType._value;
        }

        public static implicit operator PerlTokenType(int value)
        {
            return new PerlTokenType(value);
        }

        //public static bool operator ==(PerlTokenType type1, PerlTokenType type2)
        //{
        //    return type1._value == type2._value;
        //}

        //public static bool operator !=(PerlTokenType type1, PerlTokenType type2)
        //{
        //    return type1._value != type2._value;
        //}
    }
}
