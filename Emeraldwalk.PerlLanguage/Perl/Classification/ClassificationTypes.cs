using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Emeraldwalk.PerlLanguage.Perl.Classification
{
    internal static class ClassificationTypes
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.KeywordType)]
        internal static ClassificationTypeDefinition KeywordClassificationType { get; set; }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.FunctionType)]
        internal static ClassificationTypeDefinition FunctionClassificationType { get; set; }
    }
}
