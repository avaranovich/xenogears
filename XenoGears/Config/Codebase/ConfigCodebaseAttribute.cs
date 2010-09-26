using System;
using System.Diagnostics;

namespace XenoGears.Config.Codebase
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class ConfigCodebaseAttribute : Attribute
    {
    }
}