using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Emeraldwalk.PerlLanguage.Classification
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ContentType)]
    [TagType(typeof(PerlTokenTag))]
    internal sealed class PerlTokenTagProvider: ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T: ITag
        {
            return new PerlTokenTagger(buffer) as ITagger<T>;
        }
    }

    public class PerlTokenTag : ITag
    {
        public PerlTokenTag(string token)
        {
            this.Token = token;
        }

        public string Token { get; private set; }
    }

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
            foreach(SnapshotSpan span in spans)
            {
                ITextSnapshotLine snapshotLine = span.Start.GetContainingLine();
                string line = snapshotLine.GetText();
                string[] lineTokens = Regex.Split(line, @"(\#\!.*|\#.*|"".*""|'.*'|\s+|\(|\)|\{|\}|->)");
                int pos = snapshotLine.Start.Position;

                //int linePos = 0;
                foreach(string token in lineTokens)
                {
                    //if(TokenTypes.All.Contains(token, StringComparer.OrdinalIgnoreCase))
                    //{
                        SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new Span(pos, token.Length));
                        if(tokenSpan.IntersectsWith(span))
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

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ContentType)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class PerlClassifierProvider : ITaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService AggregatorFactory = null;

        [Import]
        internal IStandardClassificationService StandardClassificationService = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<PerlTokenTag> perlTokenTagAggregator = this.AggregatorFactory
                .CreateTagAggregator<PerlTokenTag>(buffer);

            return new PerlClassificationTagger(
                buffer, 
                perlTokenTagAggregator, 
                this.ClassificationTypeRegistry,
                this.StandardClassificationService) as ITagger<T>;
        }
    }

    /// <summary>
    /// Classifies tokens as keyword, comment, etc.
    /// </summary>
    internal sealed class PerlClassificationTagger : ITagger<ClassificationTag>
    {
        private ITextBuffer _buffer;
        private ITagAggregator<PerlTokenTag> _perlTokenTagAggregator;
        private IClassificationType _keywordClassificationType;
        private IClassificationType _functionClassificationType;
        private IClassificationType _operatorClassificationType;
        private IClassificationType _stringLiteralClassificationType;
        private IClassificationType _commentClassificationType;
        private IClassificationType _hashBangClassificationType;

        internal PerlClassificationTagger(
            ITextBuffer buffer, 
            ITagAggregator<PerlTokenTag> perlTokenTagAggregator, 
            IClassificationTypeRegistryService typeService,
            IStandardClassificationService standardClassificationService)
        {
            this._buffer = buffer;
            this._perlTokenTagAggregator = perlTokenTagAggregator;
            this._keywordClassificationType = standardClassificationService.Keyword;// typeService.GetClassificationType(Constants.KeywordType);
            this._functionClassificationType = typeService.GetClassificationType(Constants.FunctionType);
            this._operatorClassificationType = standardClassificationService.Operator;
            this._stringLiteralClassificationType = standardClassificationService.StringLiteral;
            this._commentClassificationType = standardClassificationService.Comment;
            this._hashBangClassificationType = standardClassificationService.ExcludedCode;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        private IClassificationType GetClassificationType(
            PerlTokenTag perlTokenTag)
        {
            if(perlTokenTag.Token == "->")
            {
                return this._operatorClassificationType;
            }
            else if(perlTokenTag.Token.StartsWith("#"))
            {
                if(perlTokenTag.Token.StartsWith("#!"))
                {
                    return this._hashBangClassificationType;
                }
                else
                {
                    return this._commentClassificationType;
                }
            }
            else if(perlTokenTag.Token.StartsWith("\"") || perlTokenTag.Token.StartsWith("'"))
            {
                return this._stringLiteralClassificationType;
            }
            else if (TokenTypes.Keywords.Contains(perlTokenTag.Token, StringComparer.OrdinalIgnoreCase))
            {
                return this._keywordClassificationType;
            }
            else if(TokenTypes.Functions.Contains(perlTokenTag.Token, StringComparer.OrdinalIgnoreCase))
            {
                return this._functionClassificationType;
            }

            return null;
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach(IMappingTagSpan<PerlTokenTag> tagSpan in this._perlTokenTagAggregator.GetTags(spans))
            {
                IClassificationType classificationType = this.GetClassificationType(tagSpan.Tag);
                if(classificationType != null)
                {
                    var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                    yield return new TagSpan<ClassificationTag>(
                        tagSpans[0],
                        new ClassificationTag(classificationType));
                }
            }
        }
    }
}
