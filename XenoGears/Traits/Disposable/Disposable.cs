using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Traits.Disposable
{
    [DebuggerNonUserCode]
    public class Disposable : IDisposable
    {
        ~Disposable()
        {
#if DEBUG
            throw new AssertionFailedException(String.Format(
                "Warning! You've leaked an object of type \"{0}\".", GetType()));
#else
            Dispose(false);
#endif
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SuppressDispose()
        {
            IsDisposed.AssertFalse();
            GC.SuppressFinalize(this);

            lock (_dispositionSyncRoot)
            {
                IsDisposed = true;
            }
        }

        public bool IsDisposed { get; private set; }
        private bool IsBeingDisposed { get; set; }
        private readonly Object _dispositionSyncRoot = new Object();

        private void Dispose(bool disposingManagedResources)
        {
            if (!IsDisposed)
            {
                // todo. this doesn't completely prevent deadlocking
                // the most robust way of implementing this routine
                // would be periodically polling some synchronization primitive
                if (IsBeingDisposed) return;

                lock (_dispositionSyncRoot)
                {
                    if (!IsDisposed)
                    {
                        try
                        {
                            IsBeingDisposed = true;

                            try
                            {
                                if (disposingManagedResources)
                                {
                                    DisposeManagedResources();
                                }
                            }
                            finally
                            {
                                DisposeUnmanagedResources();
                            }
                        }
                        finally 
                        {
                            IsBeingDisposed = false;
                            IsDisposed = true;
                        }
                    }
                }
            }
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}