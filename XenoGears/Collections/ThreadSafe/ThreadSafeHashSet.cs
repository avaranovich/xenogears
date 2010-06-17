using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Security;

namespace XenoGears.Collections.ThreadSafe
{
    [Serializable]
    [DebuggerNonUserCode]
    public class ThreadSafeHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private HashSet<T> internalHash = new HashSet<T>();

        public ThreadSafeHashSet()
        {
            this.hashLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock
        }

        [NonSerialized]
        ReaderWriterLockSlim hashLock;

        public ThreadSafeHashSet(IEnumerable<T> collection)
        {
            internalHash = new HashSet<T>(collection);
        }

        public IEqualityComparer<T> Comparer { get { return internalHash.Comparer; } }

        public int Count 
        {
            get
            {
                using (new ReadOnlyLock(this.hashLock))
                    return this.internalHash.Count;
            }
        }

        public bool Add(T item)
        {
            using (new WriteLock(this.hashLock))
                return this.internalHash.Add(item);
        }

        public void Clear()
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.Clear();
        }

        public bool Contains(T item)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.Contains(item);
        }


        public void CopyTo(T[] array)
        {
            using (new ReadOnlyLock(this.hashLock))
                this.internalHash.CopyTo(array);
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            using (new ReadOnlyLock(this.hashLock))
                this.internalHash.CopyTo(array, arrayIndex);
        }


        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            using (new ReadOnlyLock(this.hashLock))
                this.internalHash.CopyTo(array, arrayIndex, count);
        }

        public static IEqualityComparer<HashSet<T>> CreateSetComparer()
        {
            return HashSet<T>.CreateSetComparer();
        }
       
        public void ExceptWith(IEnumerable<T> other)
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.ExceptWith(other);
        }

        public HashSet<T>.Enumerator GetEnumerator()
        {
            throw new NotSupportedException("Cannot enumerate");
        }

        [SecurityCritical]
        public void IntersectWith(IEnumerable<T> other)
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.IntersectWith(other);
        }

        [SecurityCritical]
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.IsProperSubsetOf(other);
        }

        [SecurityCritical]
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.IsProperSupersetOf(other);
        }
        [SecurityCritical]
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.Overlaps(other);
        }

        public bool Remove(T item)
        {
            using (new WriteLock(this.hashLock))
                return this.internalHash.Remove(item);
        }

        public int RemoveWhere(Predicate<T> match)
        {
            using (new WriteLock(this.hashLock))
                return this.internalHash.RemoveWhere(match);
        }

        [SecurityCritical]
        public bool SetEquals(IEnumerable<T> other)
        {
            using (new ReadOnlyLock(this.hashLock))
                return this.internalHash.SetEquals(other);
        }

        [SecurityCritical]
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.SymmetricExceptWith(other);
        }

        public void TrimExcess()
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.TrimExcess();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            using (new WriteLock(this.hashLock))
                this.internalHash.UnionWith(other);
        }

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                using (new ReadOnlyLock(this.hashLock))
                    return ((ICollection<T>)this.internalHash).IsReadOnly;
            }
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
