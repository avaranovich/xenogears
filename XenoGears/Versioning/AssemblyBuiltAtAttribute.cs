using System;
using System.Diagnostics;

namespace XenoGears.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class AssemblyBuiltAtAttribute : Attribute
    {
        public String Timestamp { get; set; }

        public AssemblyBuiltAtAttribute(){}
        public AssemblyBuiltAtAttribute(String timestamp) { Timestamp = timestamp; }
    }
}