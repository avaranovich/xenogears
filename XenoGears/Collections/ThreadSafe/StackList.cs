using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XenoGears.Collections.ThreadSafe
{
    [Serializable]
    [DebuggerNonUserCode]
    public class StackList<T> : IEnumerable<T>
    {
        public StackList() { }

        protected virtual List<T> InternalList
        {
            get
            {
                if (this._InternalList == null)
                    this.InternalList = new List<T>();
                return this._InternalList;
            }
            set
            {
                this._InternalList = value;
            }
        }List<T> _InternalList;

        public virtual void Push(T obj)
        {
            this.InternalList.Add(obj);
        }

        public virtual T Pop()
        {
            T obj = default(T);

            if (this.InternalList.Count != 0)
            {
                int index = this.InternalList.Count - 1;
                obj = this.InternalList[index];
                this.InternalList.RemoveAt(index);
            }

            return obj;
        }

        public virtual T this[int index]
        {
            get
            {
                return this.InternalList[index];
            }
        }

        public virtual void RemoveAt(int index)
        {
            this.InternalList.RemoveAt(index);
        }

        public virtual void UnwindTo(T obj)
        {
            List<T> newList = new List<T>();

            foreach (T o in this.InternalList)
            {
                newList.Add(o);
                if (obj.Equals(o))
                    break;
            }

            this.InternalList = newList;
        }

        public virtual int Count
        {
            get
            {
                return this.InternalList.Count;
            }
        }

        public virtual T Peek()
        {
            return this.Peek(0);
        }

        public virtual T Peek(int offset)
        {
            T obj = default(T);

            if (this.InternalList.Count > offset)
            {
                int index = this.InternalList.Count - (offset + 1);
                obj = this.InternalList[index];
            }

            return obj;
        }

        public virtual int IndexOf(T obj)
        {
            return this.InternalList.IndexOf(obj);
        }

        public virtual void Clear()
        {
            this.InternalList.Clear();
        }

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
