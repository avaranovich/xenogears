using System;
using System.Diagnostics;

namespace XenoGears.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class AssemblyBuiltByAttribute : Attribute
    {
        public String UserHash { get; set; }

        public AssemblyBuiltByAttribute(){}
        public AssemblyBuiltByAttribute(String userHash) { UserHash = userHash; }
    }
}