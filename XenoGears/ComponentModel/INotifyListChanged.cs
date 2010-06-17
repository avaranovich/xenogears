using System;

namespace XenoGears.ComponentModel
{
    public interface INotifyListChanged
    {
        event EventHandler<ListChangeEventArgs> ListChanged;
    }
}