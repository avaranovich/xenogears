using System;
using System.Diagnostics;
using System.Threading;

namespace XenoGears.Collections.ThreadSafe
{
    [Serializable]
    [DebuggerNonUserCode]
    public class ThreadSafeStackList<T> : StackList<T>
    {
        public ThreadSafeStackList()
        {
            this.lockObj = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock
        }

        [NonSerialized]
        ReaderWriterLockSlim lockObj;

        public override void Clear()
        {
            using (new WriteLock(this.lockObj))
            {
                base.Clear();
            }
        }

        public override int Count
        {
            get
            {
                using (new ReadOnlyLock(this.lockObj))
                {
                    return base.Count;
                }
            }
        }

        public override int IndexOf(T obj)
        {
            using (new ReadOnlyLock(this.lockObj))
            {
                return base.IndexOf(obj);
            }
        }

        public override T Peek(int offset)
        {
            using (new ReadOnlyLock(this.lockObj))
            {
                return base.Peek(offset);
            }
        }

        public override T Pop()
        {
            using (new WriteLock(this.lockObj))
            {
                return base.Pop();
            }
        }

        public override void Push(T obj)
        {
            using (new WriteLock(this.lockObj))
            {
                base.Push(obj);
            }
        }

        public override void RemoveAt(int i)
        {
            using (new WriteLock(this.lockObj))
            {
                base.RemoveAt(i);
            }
        }

        public override T this[int i]
        {
            get
            {
                using (new ReadOnlyLock(this.lockObj))
                {
                    return base[i];
                }
            }
        }

        public override void UnwindTo(T obj)
        {
            using (new ReadOnlyLock(this.lockObj))
            {
                base.UnwindTo(obj);
            }
        }
    }
}
