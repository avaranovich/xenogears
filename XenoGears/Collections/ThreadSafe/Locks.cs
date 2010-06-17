using System;
using System.Diagnostics;
using System.Threading;

namespace XenoGears.Collections.ThreadSafe
{
    [DebuggerNonUserCode]
    public static class Locks
    {
        public static void GetReadLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterUpgradeableReadLock(1);
        }

        public static void GetReadOnlyLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterReadLock(1);
        }

        public static void GetWriteLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterWriteLock(1);
        }

        public static void ReleaseReadOnlyLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsReadLockHeld)
                locks.ExitReadLock();
        }

        public static void ReleaseReadLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsUpgradeableReadLockHeld)
                locks.ExitUpgradeableReadLock();
        }

        public static void ReleaseWriteLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsWriteLockHeld)
                locks.ExitWriteLock();
        }

        public static void ReleaseLock(ReaderWriterLockSlim locks)
        {
            ReleaseWriteLock(locks);
            ReleaseReadLock(locks);
            ReleaseReadOnlyLock(locks);
        }

        public static ReaderWriterLockSlim GetLockInstance()
        {
            return GetLockInstance(LockRecursionPolicy.SupportsRecursion);
        }

        public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy recursionPolicy)
        {
            return new ReaderWriterLockSlim(recursionPolicy);
        }
    }

    [DebuggerNonUserCode]
    public abstract class DevPlanetLock : IDisposable
    {
        protected ReaderWriterLockSlim _Locks;

        protected DevPlanetLock(ReaderWriterLockSlim locks)
        {
            _Locks = locks;
        }

        public abstract void Dispose();
    }

    [DebuggerNonUserCode]
    public class ReadLock : DevPlanetLock
    {
        public ReadLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadLock(this._Locks);
        }

        public override void Dispose()
        {
            Locks.ReleaseReadLock(this._Locks);
            GC.SuppressFinalize(this);
        }
    }

    [DebuggerNonUserCode]
    public class ReadOnlyLock : DevPlanetLock
    {
        public ReadOnlyLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadOnlyLock(this._Locks);
        }

        public override void Dispose()
        {
            Locks.ReleaseReadOnlyLock(this._Locks);
            GC.SuppressFinalize(this);
        }
    }

    [DebuggerNonUserCode]
    public class WriteLock : DevPlanetLock
    {
        public WriteLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetWriteLock(this._Locks);
        }

        public override void Dispose()
        {
            Locks.ReleaseWriteLock(this._Locks);
            GC.SuppressFinalize(this);
        }
    }
}
