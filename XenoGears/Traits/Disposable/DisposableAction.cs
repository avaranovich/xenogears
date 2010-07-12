using System;
using System.Diagnostics;

namespace XenoGears.Traits.Disposable
{
    [DebuggerNonUserCode]
    public class DisposableAction : IDisposable
    {
        private readonly Action _onDispose;
        public DisposableAction(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose();
        }
    }
}