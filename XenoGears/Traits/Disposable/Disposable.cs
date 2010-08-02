using System;
using System.Diagnostics;
using XenoGears.Logging;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Traits.Disposable
{
    [DebuggerNonUserCode]
    public class Disposable : IDisposable
    {
        public Disposable()
        {
            var doesnt_need_finalizer = !this.GetType().HasAttr<FinalizableAttribute>();
            if (doesnt_need_finalizer) GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
#if DEBUG
            var log = Logger.Get("XenoGears.Traits.Disposable").Warn;
            log.Warn(String.Format("Warning! You didn't explicitly dispose of an object of type \"{0}\".", GetType()));
#endif

            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }
        public bool IsBeingDisposed { get; private set; }
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