using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using XenoGears.Assertions;
using XenoGears.Traits.Disposable;

namespace XenoGears.Threading
{
    // todo. think about safe ways to implement affinity
    // since now there are a couple of places where unexpected native thread switch in the middle of the method
    // could entirely defeat our affinity implementation strategy

    [DebuggerNonUserCode]
    public static class NativeThread
    {
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId")]
        private static extern int nativeGetCurrentThreadId();
        public static int Id { get { return nativeGetCurrentThreadId(); } }

        public static IDisposable Affinitize()
        {
            return new ThreadAffinity();
        }

        public static IDisposable Affinitize(out int nativeId)
        {
            nativeId = nativeGetCurrentThreadId();
            return new ThreadAffinity();
        }

        public static IDisposable Affinitize(int nativeId)
        {
            (nativeId == nativeGetCurrentThreadId()).AssertTrue();
            return new ThreadAffinity();
        }

        [ThreadStatic] private static int _affCount;
        [DebuggerNonUserCode] private class ThreadAffinity : Disposable
        {
            private readonly int _threadId;

            public ThreadAffinity()
            {
                _threadId = nativeGetCurrentThreadId();
                if (_affCount == 0) Thread.BeginThreadAffinity();
                _affCount++;
            }

            protected override void DisposeManagedResources()
            {
                (_threadId == nativeGetCurrentThreadId()).AssertTrue();
                if (_affCount == 1) Thread.EndThreadAffinity();
                _affCount--;
            }
        }
    }
}
