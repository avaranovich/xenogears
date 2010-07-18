using System;

namespace XenoGears.ComponentModel
{
    public interface INotifyListChanging
    {
        event EventHandler<ListChangeEventArgs> ListChanging;
    }
}