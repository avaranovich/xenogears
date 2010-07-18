using System;
using System.Diagnostics;

namespace XenoGears.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    public class AssemblyBuiltOnAttribute : Attribute
    {
        public String MachineHash { get; set; }

        public AssemblyBuiltOnAttribute(){}
        public AssemblyBuiltOnAttribute(String machineHash) { MachineHash = machineHash; }
    }
}