using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl.Parsing
{
    public class TokenMatcher
    {
        public static TokenMatcher ForTokenType(TokenType tokenType)
        {
            TokenMatcher tokenMatcher = null;

            if (tokenType == PerlTokenType.CurlyBrace)
            {
                tokenMatcher = new TokenMatcher("{", "}");
            }
            else if (tokenType == PerlTokenType.SquareBrace)
            {
                tokenMatcher = new TokenMatcher("[", "]");
            }
            else if (tokenType == PerlTokenType.Parentheses)
            {
                tokenMatcher = new TokenMatcher("(", ")");
            }

            return tokenMatcher;
        }

        private readonly string _startToken;
        private readonly string _endToken;
                
        public TokenMatcher(string startToken, string endToken)
        {
            this._startToken = startToken;
            this._endToken = endToken;
        }

        private IList<Tuple<TextSpan, TextSpan>> _tokenPairs;
        public IList<Tuple<TextSpan, TextSpan>> TokenPairs
        {
            get { return this._tokenPairs ?? (this._tokenPairs = new List<Tuple<TextSpan, TextSpan>>()); }
        }

        private IEnumerable<int> GetMatchIndices(string line, string token)
        {
            int i = -1;
            while ((i = line.IndexOf(token, i + 1)) > -1)
            {
                yield return i;
            }
        }

        public void Parse(string text)
        {
            this.TokenPairs.Clear();
            Stack<TextSpan> startTokenStack = new Stack<TextSpan>();

            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for(int l = 0; l < lines.Length; ++l)
            {
                string line = lines[l];

                IEnumerator<int> startEnumerator = this.GetMatchIndices(line, this._startToken).GetEnumerator();
                IEnumerator<int> endEnumerator = this.GetMatchIndices(line, this._endToken).GetEnumerator();

                bool startHasValue = startEnumerator.MoveNext();
                bool endHasValue = endEnumerator.MoveNext();

                while (startHasValue || endHasValue)
                {
                    int start = startHasValue 
                        ? startEnumerator.Current 
                        : int.MaxValue;

                    int end = endHasValue
                        ? endEnumerator.Current
                        : int.MaxValue;

                    TextSpan token = new TextSpan
                    {
                        iStartLine = l,
                        iEndLine = l
                    };

                    // start tokens
                    if(start < end)
                    {
                        token.iStartIndex = start;
                        token.iEndIndex = start + 1;

                        startTokenStack.Push(token);

                        startHasValue = startEnumerator.MoveNext();
                    }
                    // end tokens
                    else
                    {
                        token.iStartIndex = end;
                        token.iEndIndex = end + 1;

                        if(startTokenStack.Count > 0)
                        {
                            TextSpan startToken = startTokenStack.Pop();
                            this.TokenPairs.Add(new Tuple<TextSpan, TextSpan>(
                                startToken,
                                token));
                        }

                        endHasValue = endEnumerator.MoveNext();
                    }
                }
            }
        }
    }
}
