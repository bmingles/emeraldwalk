using Emeraldwalk.PerlLanguage.Perl.Tokens;
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
using System.Threading.Tasks;

namespace Emeraldwalk.PerlLanguage.Perl.Classification
{
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
}
