using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XenoGears.Assertions;
using System.Linq;
using XenoGears.ComponentModel;
using XenoGears.Functional;
using System_INotifyPropertyChanging = System.ComponentModel.INotifyPropertyChanging;
using System_PropertyChangingEventHandler = System.ComponentModel.PropertyChangingEventHandler;
using System_PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;
using System_INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using System_PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;
using System_PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace XenoGears.Collections.Observable
{
    [DebuggerNonUserCode]
    public class ObservableList : ObservableList<Object>
    {
        public ObservableList()
        {
        }

        public ObservableList(IEnumerable enumerable)
            : base((enumerable ?? new ArrayList()).Cast<Object>())
        {
        }
    }

    [DebuggerNonUserCode]
    public class ObservableList<T> : Collection<T>, IObservableList<T>
    {
        private SimpleMonitor _monitor;

        public ObservableList()
        {
            this._monitor = new SimpleMonitor();
        }

        public ObservableList(IEnumerable<T> enumerable)
        {
            this._monitor = new SimpleMonitor();
            this.CopyFrom(enumerable.AssertNotNull());
        }

        protected override void ClearItems()
        {
            this.CheckReentrancy();
            this.OnPropertyChanging("Count");
            this.OnPropertyChanging("Item[]");
            this.OnListResetting();
            base.ClearItems();
            this.OnPropertyChanged("Item[]");
            this.OnPropertyChanged("Count");
            this.OnListReset();
        }

        private void CopyFrom(IEnumerable<T> enumerable)
        {
            var items = base.Items;
            if ((enumerable != null) && (items != null))
            {
                enumerable.ForEach(items.Add);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            this.CheckReentrancy();
            this.OnPropertyChanging("Count");
            this.OnPropertyChanging("Item[]");
            this.OnListChanging(ListChangeAction.Add, item, index);
            base.InsertItem(index, item);
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnListChanged(ListChangeAction.Add, item, index);
        }

        public void Move(int oldIndex, int newIndex)
        {
            this.MoveItem(oldIndex, newIndex);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            this.CheckReentrancy();
            var item = base[oldIndex];
            this.OnPropertyChanging("Item[]");
            this.OnListChanging(ListChangeAction.Move, item, newIndex, oldIndex);
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);
            this.OnPropertyChanged("Item[]");
            this.OnListChanged(ListChangeAction.Move, item, newIndex, oldIndex);
        }

        protected override void RemoveItem(int index)
        {
            this.CheckReentrancy();
            var item = base[index];
            this.OnPropertyChanging("Count");
            this.OnPropertyChanging("Item[]");
            this.OnListChanging(ListChangeAction.Remove, item, index);
            base.RemoveItem(index);
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnListChanged(ListChangeAction.Remove, item, index);
        }

        protected override void SetItem(int index, T item)
        {
            this.CheckReentrancy();
            var oldItem = base[index];
            this.OnPropertyChanging("Item[]");
            this.OnListChanging(ListChangeAction.Replace, oldItem, item, index);
            base.SetItem(index, item);
            this.OnPropertyChanged("Item[]");
            this.OnListChanged(ListChangeAction.Replace, oldItem, item, index);
        }

        #region Event raising and reeentrancy checks

        private Guid? _unmatchedCorrelationId = null;
        public event EventHandler<ListChangeEventArgs> ListChanged;
        public event EventHandler<ListChangeEventArgs> ListChanging;

        protected event EventHandler<PropertyChangeEventArgs> PropertyChanging;
        event System_PropertyChangingEventHandler System_INotifyPropertyChanging.PropertyChanging { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event EventHandler<PropertyChangeEventArgs> INotifyPropertyChanging.PropertyChanging { add { PropertyChanging += value; } remove { PropertyChanging -= value; } }

        protected event EventHandler<PropertyChangeEventArgs> PropertyChanged;
        event System_PropertyChangedEventHandler System_INotifyPropertyChanged.PropertyChanged { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }
        event EventHandler<PropertyChangeEventArgs> INotifyPropertyChanged.PropertyChanged { add { PropertyChanged += value; } remove { PropertyChanged -= value; } }

        protected virtual void OnListChanging(ListChangeEventArgs e)
        {
            using (this.BlockReentrancy())
            {
                // if an exception is thrown, changed handler 
                // might not have a chance on resetting the corrid
                // so just commenting this

                // _unmatchedCorrelationId.AssertNull();

                _unmatchedCorrelationId = Guid.NewGuid();
                e.CorrelationId = _unmatchedCorrelationId.Value;

                if (this.ListChanging != null)
                {
                    this.ListChanging(this, e);
                }
            }
        }

        private void OnListChanging(ListChangeAction action, object item, int index)
        {
            this.OnListChanging(new ListChangeEventArgs(action, item, index));
        }

        private void OnListChanging(ListChangeAction action, object item, int index, int oldIndex)
        {
            this.OnListChanging(new ListChangeEventArgs(action, item, index, oldIndex));
        }

        private void OnListChanging(ListChangeAction action, object oldItem, object newItem, int index)
        {
            this.OnListChanging(new ListChangeEventArgs(action, newItem, oldItem, index));
        }

        private void OnListResetting()
        {
            this.OnListChanging(new ListChangeEventArgs(ListChangeAction.Reset));
        }

        protected virtual void OnListChanged(ListChangeEventArgs e)
        {
            using (this.BlockReentrancy())
            {
                _unmatchedCorrelationId.AssertNotNull();
                e.CorrelationId = _unmatchedCorrelationId.Value;
                _unmatchedCorrelationId = null;

                if (this.ListChanged != null)
                {
                    this.ListChanged(this, e);
                }
            }
        }

        private void OnListChanged(ListChangeAction action, object item, int index)
        {
            this.OnListChanged(new ListChangeEventArgs(action, item, index));
        }

        private void OnListChanged(ListChangeAction action, object item, int index, int oldIndex)
        {
            this.OnListChanged(new ListChangeEventArgs(action, item, index, oldIndex));
        }

        private void OnListChanged(ListChangeAction action, object oldItem, object newItem, int index)
        {
            this.OnListChanged(new ListChangeEventArgs(action, newItem, oldItem, index));
        }

        private void OnListReset()
        {
            this.OnListChanged(new ListChangeEventArgs(ListChangeAction.Reset));
        }

        protected virtual void OnPropertyChanging(PropertyChangeEventArgs e)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangeEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            this.OnPropertyChanging(new PropertyChangeEventArgs(propertyName));
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangeEventArgs(propertyName));
        }

        protected IDisposable BlockReentrancy()
        {
            this._monitor.Enter();
            return this._monitor;
        }

        protected void CheckReentrancy()
        {
            if (this._monitor.Busy)
            {
                var reenteredByChanged = this.ListChanged != null && this.ListChanged.GetInvocationList().Length > 1;
                var reenteredByChanging = this.ListChanging != null && this.ListChanging.GetInvocationList().Length > 1;

                // reentrancy not allowed when processing events
                (reenteredByChanged || reenteredByChanging).AssertFalse();
            }
        }

        [Serializable]
        [DebuggerNonUserCode]
        private class SimpleMonitor : IDisposable
        {
            private int _busyCount;

            public void Dispose()
            {
                this._busyCount--;
            }

            public void Enter()
            {
                this._busyCount++;
            }

            public bool Busy
            {
                get
                {
                    return (this._busyCount > 0);
                }
            }
        }

        #endregion

        #region IObservableList<T> boilerplate

        bool IObservableList<T>.IsReadOnly { get { return false; } }

        #endregion
    }
}
