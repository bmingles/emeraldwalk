using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emeraldwalk.PerlLanguage.Classification
{
    internal static class PerlContentTypeDefinition
    {
        /// <summary>
        /// Perl content type
        /// </summary>
        [Export]
        [BaseDefinition("code")]
        [Name(Constants.ContentType)]
        internal static ContentTypeDefinition ContentTypeDefinition { get; set; }

        /// <summary>
        /// Perl .pl file extension
        /// </summary>
        [Export]
        [FileExtension(".pl")]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition FileExtensionDefinition { get; set; }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.KeywordType)]
        internal static ClassificationTypeDefinition KeywordClassificationType { get; set; }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.FunctionType)]
        internal static ClassificationTypeDefinition FunctionClassificationType { get; set; }
    }
}
