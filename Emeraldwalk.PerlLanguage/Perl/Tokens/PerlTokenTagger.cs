using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Emeraldwalk.PerlLanguage.Perl.Tokens
{
    internal sealed class PerlTokenTagger : ITagger<PerlTokenTag>
    {
        private ITextBuffer _textBuffer;

        internal PerlTokenTagger(ITextBuffer textBuffer)
        {
            this._textBuffer = textBuffer;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<PerlTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan span in spans)
            {
                ITextSnapshotLine snapshotLine = span.Start.GetContainingLine();
                string line = snapshotLine.GetText();
                string[] lineTokens = Regex.Split(line, @"(\#\!.*|\#.*|""[^""]*""|'[^']*'|\s+|\(|\)|\{|\}|->)");
                int pos = snapshotLine.Start.Position;

                //int linePos = 0;
                foreach (string token in lineTokens)
                {
                    //if(TokenTypes.All.Contains(token, StringComparer.OrdinalIgnoreCase))
                    //{
                    SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new Span(pos, token.Length));
                    if (tokenSpan.IntersectsWith(span))
                    {
                        yield return new TagSpan<PerlTokenTag>(
                            tokenSpan,
                            new PerlTokenTag(token));
                    }
                    //}

                    //linePos += token.Length;
                    pos += token.Length;
                }
            }
        }
    }
}
