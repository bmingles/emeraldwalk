using Microsoft.VisualStudio.Text.Tagging;

namespace Emeraldwalk.PerlLanguage.Perl.Tokens
{
    public class PerlTokenTag : ITag
    {
        public PerlTokenTag(string token)
        {
            this.Token = token;
        }

        public string Token { get; private set; }
    }
}
