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
    public class PerlSource : Source
    {
        public PerlSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer)
        {
            //this.methodData = this.CreateMethodData();
        }

        public override CommentInfo GetCommentFormat()
        {
            return new CommentInfo
            {
                LineStart = "#",
                BlockStart = "#",
                BlockEnd = "",
                UseLineComments = true
            };
        }

        //private MethodData methodData;

        //public override void OnCommand(IVsTextView textView, Microsoft.VisualStudio.VSConstants.VSStd2KCmdID command, char ch)
        //{
        //    //base.OnCommand(textView, command, ch);
        //    //return;

        //    if (textView == null || this.LanguageService == null || !this.LanguageService.Preferences.EnableCodeSense)
        //    {
        //        return;
        //    }
        //    bool flag = command == VSConstants.VSStd2KCmdID.BACKSPACE || command == VSConstants.VSStd2KCmdID.BACKTAB || command == VSConstants.VSStd2KCmdID.LEFT || command == VSConstants.VSStd2KCmdID.LEFT_EXT;
        //    int line;
        //    int num;
        //    textView.GetCaretPos(out line, out num);
        //    //NativeMethods.ThrowOnFailure(textView.GetCaretPos(out line, out num));
        //    TokenInfo tokenInfo = this.GetTokenInfo(line, num);
        //    TokenTriggers trigger = tokenInfo.Trigger;
        //    if ((trigger & TokenTriggers.MemberSelect) != TokenTriggers.None && command == VSConstants.VSStd2KCmdID.TYPECHAR)
        //    {
        //        ParseReason reason = ((trigger & TokenTriggers.MatchBraces) == TokenTriggers.MatchBraces) ? ParseReason.MemberSelectAndHighlightBraces : ParseReason.MemberSelect;
        //        this.Completion(textView, tokenInfo, reason);
        //    }
        //    else
        //    {
        //        if ((trigger & TokenTriggers.MatchBraces) != TokenTriggers.None && this.LanguageService.Preferences.EnableMatchBraces && command != VSConstants.VSStd2KCmdID.BACKSPACE && (command == VSConstants.VSStd2KCmdID.TYPECHAR || this.LanguageService.Preferences.EnableMatchBracesAtCaret))
        //        {
        //            this.MatchBraces(textView, line, num, tokenInfo);
        //        }
        //    }
        //    if ((trigger & TokenTriggers.MethodTip) != TokenTriggers.None && this.methodData.IsDisplayed)
        //    {
        //        if ((trigger & TokenTriggers.MethodTip) == TokenTriggers.ParameterNext)
        //        {
        //            this.methodData.AdjustCurrentParameter((flag && num > 0) ? -1 : 1);
        //            return;
        //        }
        //        this.MethodTip(textView, line, (flag && num > 0) ? (num - 1) : num, tokenInfo);
        //        return;
        //    }
        //    else
        //    {
        //        if ((trigger & TokenTriggers.MethodTip) != TokenTriggers.None && command == VSConstants.VSStd2KCmdID.TYPECHAR && this.LanguageService.Preferences.ParameterInformation)
        //        {
        //            this.MethodTip(textView, line, num, tokenInfo);
        //            return;
        //        }
        //        if (this.methodData.IsDisplayed)
        //        {
        //            this.MethodTip(textView, line, num, tokenInfo);
        //        }
        //        return;
        //    }
        //}

        //public override void MatchBraces(IVsTextView textView, int line, int index, TokenInfo info)
        //{
        //    base.MatchBraces(textView, line, index, info);
        //}
    }
}
