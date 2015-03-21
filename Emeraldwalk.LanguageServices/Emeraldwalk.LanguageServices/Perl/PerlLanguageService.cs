using Emeraldwalk.Emeraldwalk_LanguageServices.Perl.Parsing;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    public class PerlLanguageService: LanguageService
    {
        public const string SERVICE_NAME = "Perl Language Service";
        public const string LANGUAGE_NAME = "Perl Language";
        public const int LANGUAGE_RESOURCE_ID = 106; //resource id to localized language name (really not sure if this is right, but 106 is referenced in samples such as this one: https://msdn.microsoft.com/fr-fr/library/microsoft.visualstudio.package.languageservice(v=vs.90).aspx)

        private LanguagePreferences _languagePreferences;
        private IScanner _scanner;
       
        public PerlLanguageService()
        {
        }

        private IDictionary<PerlTokenColor, IVsColorableItem> _customColorableItems;
        private IDictionary<PerlTokenColor, IVsColorableItem> CustomColorableItems
        {
            get
            {
                return this._customColorableItems ?? (_customColorableItems = new Dictionary<PerlTokenColor, IVsColorableItem>
                {
                    { 
                        PerlTokenColor.Shebang, 
                        new ColorableItem("Shebang (Perl Language)",
                            "Shebang",
                            COLORINDEX.CI_LIGHTGRAY,
                            COLORINDEX.CI_SYSPLAINTEXT_BK,
                            System.Drawing.Color.LightGray,
                            System.Drawing.Color.Empty,
                            FONTFLAGS.FF_DEFAULT)
                    },
                    {
                        PerlTokenColor.Function,
                        new ColorableItem("Function (Perl Language)",
                            "Function",
                            COLORINDEX.CI_PURPLE,
                            COLORINDEX.CI_SYSPLAINTEXT_BK,
                            System.Drawing.Color.Purple,
                            System.Drawing.Color.Empty,
                            FONTFLAGS.FF_DEFAULT)
                    }
                });
            }
        }

        public override string Name
        {
            get { return SERVICE_NAME; }
        }

        public override string GetFormatFilterList()
        {
            return "Perl Files (*.pl)|*.pl|Perl Modules (*.pm)|*.pm";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if(this._languagePreferences == null)
            {
                this._languagePreferences = new LanguagePreferences(
                    this.Site,
                    typeof(LanguageService).GUID,
                    this.Name);

                this._languagePreferences.Init();
                this._languagePreferences.EnableCodeSense = true;
                this._languagePreferences.EnableMatchBraces = true;
                this._languagePreferences.EnableMatchBracesAtCaret = true;
                this._languagePreferences.EnableShowMatchingBrace = true;
                this._languagePreferences.AutoOutlining = true;
                this._languagePreferences.EnableCommenting = true;
                this._languagePreferences.LineNumbers = true;
            }

            return this._languagePreferences;
        }
                
        public override IScanner GetScanner(IVsTextLines buffer)
        {
            return this._scanner ?? (this._scanner = new PerlScanner(buffer));
        }

        public override Source CreateSource(IVsTextLines buffer)
        {
            return new PerlSource(this, buffer, this.GetColorizer(buffer));
        }

        public override void OnIdle(bool periodic)
        {
            Source source = this.GetSource(this.LastActiveTextView);

            // Found similar comment in multiple places online
            // References say it originated in IronPython langage sample
            if(source != null && source.LastParseTime >= Int32.MaxValue >> 12)
            {
                source.LastParseTime = 0;
            }
            base.OnIdle(periodic);
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            Source source = this.GetSource(req.FileName);
            switch(req.Reason)
            {
                //case ParseReason.Check:
                case ParseReason.HighlightBraces:
                    TokenMatcher tokenMatcher = TokenMatcher.ForTokenType(req.TokenInfo.Type);// ?? TokenMatcher.ForTokenType(PerlTokenType.CurlyBrace);
                    
                    if (tokenMatcher != null)
                    {
                        tokenMatcher.Parse(req.Text);
                        foreach (Tuple<TextSpan, TextSpan> tokenPair in tokenMatcher.TokenPairs)
                        {                            
                            //if(req.Sink.HiddenRegions)//req.TokenInfo.Type == PerlTokenType.CurlyBrace)
                            //{
                            //    TextSpan hiddenSpan = new TextSpan
                            //    {
                            //        iStartLine = tokenPair.Item1.iStartLine,
                            //        iStartIndex = tokenPair.Item1.iStartIndex,
                            //        iEndLine = tokenPair.Item2.iEndLine,
                            //        iEndIndex = tokenPair.Item2.iEndIndex
                            //    };                                
                            //    req.Sink.AddHiddenRegion(hiddenSpan);
                            //    req.Sink.ProcessHiddenRegions = true;
                            //}
                            req.Sink.MatchPair(tokenPair.Item1, tokenPair.Item2, 1);
                        }
                    }
                    break;

                default:
                    break;
            }

            return new PerlAuthoringScope();
        }

        public override int GetItemCount(out int count)
        {
            count = this.CustomColorableItems.Keys.Count + 6; // custom + 6 default
            return VSConstants.S_OK;
        }

        public override int GetColorableItem(int index, out IVsColorableItem item)
        {
            PerlTokenColor perlTokenColor = (PerlTokenColor)index;

            item = null;
            if (!this.CustomColorableItems.ContainsKey(perlTokenColor))
            {
                return VSConstants.E_NOTIMPL;
            }

            item = this.CustomColorableItems[perlTokenColor];
            return VSConstants.S_OK;
        }
    }
}
