using System.Collections;
using System.Collections.Generic;
using XenoGears.ComponentModel;

namespace XenoGears.Collections.Observable
{
    public interface IObservableList : IList, 
        INotifyListChanging, INotifyListChanged,
        INotifyPropertyChanging, INotifyPropertyChanged
    {
    }

    public interface IObservableList<T> : IList<T>, IObservableList
    {
        new void Clear();
        new int Count { get; }
        new bool IsReadOnly { get; }
        new T this[int index] { get; set; }
        new void RemoveAt(int index);
    }
}