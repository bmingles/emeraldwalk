using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    public class PerlScanner: IScanner
    {
        private IVsTextBuffer _buffer;
        private string _source;
        private int _offset;
        private int _tokenIndex;
        private IList<string> _sourceLineTokens;

        public PerlScanner(IVsTextBuffer buffer)
        {
            this._buffer = buffer;
        }

        public void SetSource(string source, int offset)
        {
            this._source = source.Substring(offset);
            this._offset = 0;
            this._tokenIndex = 0;
            this._sourceLineTokens = Regex
                .Split(source, @"(\#\!.*|\#.*|""[^""]*""|'[^']*'|\s+|\(|\)|\{|\}|->)")
                .Where(token => token.Length > 0)
                .ToList();

            System.Diagnostics.Debug.WriteLine("--------------");
            System.Diagnostics.Debug.WriteLine(this._source);
            //foreach(string token in this._sourceLineTokens)
            //{
            //    System.Diagnostics.Debug.WriteLine(token);
            //}
        }

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            if (this._tokenIndex < this._sourceLineTokens.Count)
            {
                string token = this._sourceLineTokens[this._tokenIndex];
                //System.Diagnostics.Debug.Write(string.Format("{0}||{1}|{2}||", this._tokenIndex, tokenInfo.StartIndex, tokenInfo.EndIndex));
                tokenInfo.StartIndex = this._offset;
                tokenInfo.EndIndex = tokenInfo.StartIndex + token.Length - 1;
                this._offset = tokenInfo.EndIndex + 1;

                if(token.StartsWith("#"))
                {
                    if(token.StartsWith("#!"))
                    {
                        tokenInfo.Type = PerlTokenType.Shebang;
                        tokenInfo.Color = PerlTokenColor.Shebang;
                    }
                    else
                    {
                        tokenInfo.Type = PerlTokenType.LineComment;
                        tokenInfo.Color = PerlTokenColor.Comment;
                    }
                }
                else if (token.StartsWith("\"") || token.StartsWith("'"))
                {
                    tokenInfo.Type = PerlTokenType.String;
                    tokenInfo.Color = PerlTokenColor.String;
                }
                else if(token.Length == 1 && "[]".IndexOf(token[0]) > -1)
                {
                    tokenInfo.Type = PerlTokenType.SquareBrace;
                    tokenInfo.Color = PerlTokenColor.Text;
                    tokenInfo.Trigger = TokenTriggers.MatchBraces;
                }
                else if (token.Length == 1 && "{}".IndexOf(token[0]) > -1)
                {
                    tokenInfo.Type = PerlTokenType.CurlyBrace;
                    tokenInfo.Color = PerlTokenColor.Text;
                    tokenInfo.Trigger = TokenTriggers.MatchBraces;
                }
                else if (token.Length == 1 && "()".IndexOf(token[0]) > -1)
                {
                    tokenInfo.Type = PerlTokenType.Parentheses;
                    tokenInfo.Color = PerlTokenColor.Text;
                    tokenInfo.Trigger = TokenTriggers.MatchBraces;
                }
                else if(PerlTokens.NumberTokenPattern.IsMatch(token))
                {
                    tokenInfo.Type = PerlTokenType.Literal;
                    tokenInfo.Color = PerlTokenColor.Number;
                }
                else if (PerlTokens.Operators.Contains(token, StringComparer.OrdinalIgnoreCase))
                {
                    tokenInfo.Type = PerlTokenType.Operator;
                    tokenInfo.Color = PerlTokenColor.Identifier;
                }
                else if (PerlTokens.Keywords.Contains(token, StringComparer.OrdinalIgnoreCase))
                {
                    tokenInfo.Type = PerlTokenType.Keyword;
                    tokenInfo.Color = PerlTokenColor.Keyword;
                }
                else if (PerlTokens.Functions.Contains(token, StringComparer.OrdinalIgnoreCase))
                {
                    tokenInfo.Type = PerlTokenType.Function;
                    tokenInfo.Color = PerlTokenColor.Function;
                }
                else
                {
                    tokenInfo.Type = PerlTokenType.Text;
                    tokenInfo.Color = PerlTokenColor.Text;
                }

                //System.Diagnostics.Debug.WriteLine(string.Format("{0}|{1}|{2}",
                //    tokenInfo.StartIndex, 
                //    tokenInfo.EndIndex,
                //    token));
                ++this._tokenIndex;
                
                return true;
            }
            return false;
        }        
    }
}
