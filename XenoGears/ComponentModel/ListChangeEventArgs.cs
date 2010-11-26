using System;
using System.Collections;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.ComponentModel
{
    // cannot make this typed, because this will make
    // INotifyListChanged/INotifyListChanging unusable in general case
    // c'mon gife CLR 4.0 so that INLC<T> can be implicitly cast to INLC<Object>

    [DebuggerNonUserCode]
    public class ListChangeEventArgs : EventArgs
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id { get { return _id; } }

        private bool _correlationIdSet = false;
        private Guid _correlationId;
        public Guid CorrelationId
        {
            get
            {
                _correlationIdSet.AssertTrue();
                return _correlationId;
            }
            set
            {
                _correlationIdSet.AssertFalse();
                _correlationId = value;
                _correlationIdSet = true;
            }
        }

        public ListChangeAction Action { get; private set; }
        private IList _newItems = new ArrayList();
        public IList NewItems { get { return _newItems; } private set { _newItems = value; } }
        public int NewStartingIndex { get; private set; }
        private IList _oldItems = new ArrayList();
        public IList OldItems { get { return _oldItems; } private set { _oldItems = value; } }
        public int OldStartingIndex { get; private set; }

        public override string ToString()
        {
            return String.Format("{0}: +[{1}], -[{2}]",
                Action, NewItems.StringJoin(), OldItems.StringJoin());
        }

        public ListChangeEventArgs(ListChangeAction action)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;
            (action == ListChangeAction.Reset).AssertTrue();
            this.InitializeAdd(action, null, -1);
        }

        public ListChangeEventArgs(ListChangeAction action, IList changedItems)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            ((action == ListChangeAction.Add) ||
                (action == ListChangeAction.Remove) ||
                    (action == ListChangeAction.Reset)).AssertTrue();

            if (action == ListChangeAction.Reset)
            {
                changedItems.AssertNull();
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                changedItems.AssertNotNull();
                this.InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        public ListChangeEventArgs(ListChangeAction action, object changedItem)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            ((action == ListChangeAction.Add) ||
                (action == ListChangeAction.Remove) ||
                    (action == ListChangeAction.Reset)).AssertTrue();

            if (action == ListChangeAction.Reset)
            {
                changedItem.AssertNull();
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, -1);
            }
        }

        public ListChangeEventArgs(ListChangeAction action, IList newItems, IList oldItems)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Replace).AssertTrue();
            (newItems != null && oldItems != null).AssertTrue();
            this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        public ListChangeEventArgs(ListChangeAction action, IList changedItems, int startingIndex)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            ((action == ListChangeAction.Add) ||
                (action == ListChangeAction.Remove) ||
                    (action == ListChangeAction.Reset)).AssertTrue();

            if (action == ListChangeAction.Reset)
            {
                changedItems.AssertNull();
                (startingIndex == -1).AssertTrue();
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                changedItems.AssertNotNull();
                (startingIndex != -1).AssertTrue();
                this.InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }

        public ListChangeEventArgs(ListChangeAction action, object changedItem, int index)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            ((action == ListChangeAction.Add) ||
                (action == ListChangeAction.Remove) ||
                    (action == ListChangeAction.Reset)).AssertTrue();

            if (action == ListChangeAction.Reset)
            {
                changedItem.AssertNull();
                (index == -1).AssertTrue();
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, index);
            }
        }

        public ListChangeEventArgs(ListChangeAction action, object newItem, object oldItem)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Replace).AssertTrue();
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
        }

        public ListChangeEventArgs(ListChangeAction action, IList newItems, IList oldItems, int startingIndex)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Replace).AssertTrue();
            (newItems != null && oldItems != null).AssertTrue();
            this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
        }

        public ListChangeEventArgs(ListChangeAction action, IList changedItems, int index, int oldIndex)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Move).AssertTrue();
            (index >= 0).AssertTrue();
            this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }

        public ListChangeEventArgs(ListChangeAction action, object changedItem, int index, int oldIndex)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Move).AssertTrue();
            (index >= 0).AssertTrue();

            var newItems = new object[] { changedItem };
            this.InitializeMoveOrReplace(action, newItems, newItems, index, oldIndex);
        }

        public ListChangeEventArgs(ListChangeAction action, object newItem, object oldItem, int index)
        {
            this.NewStartingIndex = -1;
            this.OldStartingIndex = -1;

            (action == ListChangeAction.Replace).AssertTrue();
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
        }

        private void InitializeAddOrRemove(ListChangeAction action, IList changedItems, int startingIndex)
        {
            if (action == ListChangeAction.Add)
            {
                this.InitializeAdd(action, changedItems, startingIndex);
            }
            else if (action == ListChangeAction.Remove)
            {
                this.InitializeRemove(action, changedItems, startingIndex);
            }
            else
            {
                AssertionHelper.Fail();
            }
        }

        private void InitializeMoveOrReplace(ListChangeAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            this.InitializeAdd(action, newItems, startingIndex);
            this.InitializeRemove(action, oldItems, oldStartingIndex);
        }

        private void InitializeAdd(ListChangeAction action, IList newItems, int newStartingIndex)
        {
            this.Action = action;
            this.NewItems = ArrayList.ReadOnly(newItems ?? new ArrayList());
            this.NewStartingIndex = newStartingIndex;
        }

        private void InitializeRemove(ListChangeAction action, IList oldItems, int oldStartingIndex)
        {
            this.Action = action;
            this.OldItems = ArrayList.ReadOnly(oldItems ?? new ArrayList());
            this.OldStartingIndex = oldStartingIndex;
        }
    }
}