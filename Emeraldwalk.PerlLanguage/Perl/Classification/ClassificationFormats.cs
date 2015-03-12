using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emeraldwalk.PerlLanguage.Perl.Classification
{
    /// <summary>
    /// Keyword formatting
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.KeywordType)]
    [Name(Constants.KeywordType)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class KeywordClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public KeywordClassificationFormatDefinition()
        {
            this.DisplayName = "Perl Keyword";
            this.ForegroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Function formatting
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.FunctionType)]
    [Name(Constants.FunctionType)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class FunctionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        public FunctionClassificationFormatDefinition()
        {
            this.DisplayName = "Perl Function";
            this.ForegroundColor = Colors.Purple;
        }
    }
}
