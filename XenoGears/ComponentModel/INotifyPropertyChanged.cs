using System;
using System_INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace XenoGears.ComponentModel
{
    public interface INotifyPropertyChanged : System_INotifyPropertyChanged
    {
        new event EventHandler<PropertyChangeEventArgs> PropertyChanged;
    }
}