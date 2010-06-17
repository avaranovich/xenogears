using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace XenoGears.Collections.ThreadSafe
{
    [DebuggerNonUserCode]
    public class ThreadSafeQueue<T>
    {
        //This is the internal dictionary that we are wrapping
        Queue<T> queue = new Queue<T>();

        public ThreadSafeQueue()
        {
            this.objLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock
        }

        [NonSerialized]
        ReaderWriterLockSlim objLock;

        public int Count
        {
            get
            {
                using (new ReadOnlyLock(this.objLock))
                {
                    return this.queue.Count;
                }
            }
        }

        public void Clear()
        {
            using (new WriteLock(this.objLock))
            {
                this.queue.Clear(); ;
            }
        }

        public bool Contains(T item)
        {
            using (new ReadOnlyLock(this.objLock))
            {
                return this.queue.Contains(item);
            }
        }

        public bool TryDequeue(out T obj)
        {
            using (new ReadLock(this.objLock))
            {
                if (this.queue.Count != 0)
                {
                    obj = this.Dequeue();
                    return true;
                }
            }

            obj = default(T);
            return false;
        }

        public T Dequeue()
        {
            using (new WriteLock(this.objLock))
            {
                return this.queue.Dequeue();
            }
        }

        public virtual void Enqueue(T item)
        {
            using (new WriteLock(this.objLock))
            {
                this.queue.Enqueue(item);
            }
        }

        public virtual void Enqueue(IEnumerable<T> items)
        {
            using (new WriteLock(this.objLock))
            {
                foreach (T item in items)
                    this.queue.Enqueue(item);
            }
        }

        public T Peek()
        {
            using (new ReadOnlyLock(this.objLock))
            {
                return this.queue.Peek();
            }
        }

    }
}
