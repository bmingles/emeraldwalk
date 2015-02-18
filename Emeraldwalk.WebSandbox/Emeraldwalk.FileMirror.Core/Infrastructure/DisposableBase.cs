using System;

namespace Emeraldwalk.FileMirror.Core.Infrastructure
{
    public abstract class DisposableBase: IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}
