using System;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using XenoGears.ComponentModel;
using XenoGears.Functional;

namespace XenoGears.Collections.Observable
{
    [DebuggerNonUserCode]
    public static class ChangesetHelper
    {
        public static void Apply(this IList list, ListChangeEventArgs changes)
        {
            var si = changes.NewStartingIndex == -1 ? list.Count : changes.NewStartingIndex;

            switch (changes.Action)
            {
                case ListChangeAction.Add:
                    changes.NewItems.ForEach((Object el, int i) => list.Insert(si + i, el));
                    break;

                case ListChangeAction.Remove:
                    changes.OldItems.Cast<Object>().ForEach(list.Remove);
                    break;

                case ListChangeAction.Replace:
                case ListChangeAction.Move:
                    changes.NewItems.ForEach((Object el, int i) => list.Insert(si + i, el));
                    changes.OldItems.Cast<Object>().ForEach(list.Remove);
                    break;

                case ListChangeAction.Reset:
                    list.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(changes.Action.ToString());
            }
        }

        public static void Apply<T>(this IList<T> list, ListChangeEventArgs changes)
        {
            var si = changes.NewStartingIndex == -1 ? list.Count : changes.NewStartingIndex;

            switch (changes.Action)
            {
                case ListChangeAction.Add:
                    changes.NewItems.ForEach((Object el, int i) => list.Insert(si + i, (T)el));
                    break;

                case ListChangeAction.Remove:
                    changes.OldItems.Cast<T>().ForEach(el => list.Remove(el));
                    break;

                case ListChangeAction.Replace:
                case ListChangeAction.Move:
                    changes.NewItems.ForEach((Object el, int i) => list.Insert(si + i, (T)el));
                    changes.OldItems.Cast<T>().ForEach(el => list.Remove(el));
                    break;

                case ListChangeAction.Reset:
                    list.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(changes.Action.ToString());
            }
        }
    }
}