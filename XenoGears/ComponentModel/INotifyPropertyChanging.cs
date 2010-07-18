using System;
using System_INotifyPropertyChanging = System.ComponentModel.INotifyPropertyChanging;

namespace XenoGears.ComponentModel
{
    public interface INotifyPropertyChanging : System_INotifyPropertyChanging
    {
        new event EventHandler<PropertyChangeEventArgs> PropertyChanging;
    }
}