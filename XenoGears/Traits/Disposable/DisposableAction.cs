using System;
using System.Diagnostics;
using XenoGears.Functional;
using System.Linq;

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

        private readonly IDisposable _disposable;
        public DisposableAction(IDisposable disposable)
            : this(disposable.Dispose)
        {
            _disposable = disposable;
        }

        public void Dispose()
        {
            _onDispose();
        }

        public static DisposableAction operator +(DisposableAction d1, DisposableAction d2)
        {
            if (d1 == null) return d2;
            if (d2 == null) return d1;
            return new DisposableChain(d1, d2);
        }

        public static DisposableAction operator +(DisposableAction d1, IDisposable d2)
        {
            if (d2 == null) return d1;
            if (d1 == null) return new DisposableAction(d2);
            return new DisposableChain(d1, new DisposableAction(d2));
        }

        public static DisposableAction operator +(IDisposable d1, DisposableAction d2)
        {
            if (d1 == null) return d2;
            if (d2 == null) return new DisposableAction(d1);
            return new DisposableChain(new DisposableAction(d1), d2);
        }

        public static DisposableAction operator -(DisposableAction d1, DisposableAction d2)
        {
            if (d1 == null) return null;
            if (d2 == null) return d1;

            var c1 = d1 as DisposableChain;
            var a1 = c1 ?? d1.MkArray().AsEnumerable();
            var c2 = d2 as DisposableChain;
            var a2 = c2 ?? d2.MkArray().AsEnumerable();

            var a = a1.Except(a2);
            return new DisposableChain(a);
        }

        public static DisposableAction operator -(DisposableAction d1, IDisposable d2)
        {
            if (d1 == null) return null;
            if (d2 == null) return d1;

            var c1 = d1 as DisposableChain;
            var a1 = c1 ?? d1.MkArray().AsEnumerable();

            var a = a1.Where(d => d._disposable != d2);
            return new DisposableChain(a);
        }
    }
}