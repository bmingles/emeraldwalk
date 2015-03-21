using Microsoft.VisualStudio.OLE.Interop;
using System;
using System.Runtime.InteropServices;

namespace Emeraldwalk.Emeraldwalk_LanguageServices.Perl
{
    public delegate void OnIdleDelegate(bool periodic);

    /// <summary>
    /// Component used to run LanguageService during idle times.
    /// General idea based on code here: https://msdn.microsoft.com/en-us/library/bb166498.aspx
    /// </summary>
    public class IdleHandler: IOleComponent, IDisposable
    {
        private uint _componentId;
        private IOleComponentManager _oleComponentManager;
        private OnIdleDelegate _onIdleHandler;

        public IdleHandler(
            IOleComponentManager oleComponentManager,
            OnIdleDelegate onIdleHandler)
        {
            this._oleComponentManager = oleComponentManager;
            this._onIdleHandler = onIdleHandler;
            this.RegisterComponent();
        }

        private void RegisterComponent()
        {
            OLECRINFO olecrinfo = new OLECRINFO
            {
                cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO)),
                grfcrf = (uint)_OLECRF.olecrfNeedIdleTime |
                         (uint)_OLECRF.olecrfNeedPeriodicIdleTime,
                grfcadvf = (uint)_OLECADVF.olecadvfModal |
                           (uint)_OLECADVF.olecadvfRedrawOff |
                           (uint)_OLECADVF.olecadvfWarningsOff,
                uIdleTimeInterval = 1000
            };

            uint componentId;
            this._oleComponentManager.FRegisterComponent(
                this,
                new OLECRINFO[] { olecrinfo },
                out componentId);
            this._componentId = componentId;
        }

        public int FDoIdle(uint grfidlef)
        {
            bool periodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
            this._onIdleHandler(periodic);
            return 0;
        }

        #region Default Implementations

        public int FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked)
        {
            return 1;
        }

        public int FPreTranslateMessage(MSG[] pMsg)
        {
            return 0;
        }

        public int FQueryTerminate(int fPromptUser)
        {
            return 1;
        }

        public int FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved)
        {
            return IntPtr.Zero;
        }

        public void OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved)
        {}

        public void OnAppActivate(int fActive, uint dwOtherThreadID)
        {}

        public void OnEnterState(uint uStateID, int fEnter)
        {}

        public void OnLoseActivation()
        {}

        public void Terminate()
        {}

        #endregion

        public bool Disposed { get; private set; }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if(this.Disposed)
            {
                return;
            }

            this._oleComponentManager.FRevokeComponent(this._componentId);
            this._componentId = 0;
            this.Disposed = true;
        }
    }
}
