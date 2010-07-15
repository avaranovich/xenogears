using System;
using System.Diagnostics;

namespace XenoGears.Threading
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class WorkerThreadAttribute : Attribute
    {
        public String Name { get; set; }
        public bool IsAffined { get; set; }
    }
}