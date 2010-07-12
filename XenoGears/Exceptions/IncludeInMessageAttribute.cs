using System;
using System.Diagnostics;

namespace XenoGears.Exceptions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class IncludeInMessageAttribute : Attribute
    {
    }
}