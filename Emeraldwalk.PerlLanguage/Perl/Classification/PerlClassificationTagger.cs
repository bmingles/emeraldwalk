using Emeraldwalk.PerlLanguage.Perl.Tokens;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Emeraldwalk.PerlLanguage.Perl.Classification
{
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
            if (perlTokenTag.Token == "->")
            {
                return this._operatorClassificationType;
            }
            else if (perlTokenTag.Token.StartsWith("#"))
            {
                if (perlTokenTag.Token.StartsWith("#!"))
                {
                    return this._hashBangClassificationType;
                }
                else
                {
                    return this._commentClassificationType;
                }
            }
            else if (perlTokenTag.Token.StartsWith("\"") || perlTokenTag.Token.StartsWith("'"))
            {
                return this._stringLiteralClassificationType;
            }
            else if (TokenTypes.Keywords.Contains(perlTokenTag.Token, StringComparer.OrdinalIgnoreCase))
            {
                return this._keywordClassificationType;
            }
            else if (TokenTypes.Functions.Contains(perlTokenTag.Token, StringComparer.OrdinalIgnoreCase))
            {
                return this._functionClassificationType;
            }

            return null;
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (IMappingTagSpan<PerlTokenTag> tagSpan in this._perlTokenTagAggregator.GetTags(spans))
            {
                IClassificationType classificationType = this.GetClassificationType(tagSpan.Tag);
                if (classificationType != null)
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
