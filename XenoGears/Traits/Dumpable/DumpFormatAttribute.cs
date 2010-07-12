using System;
using System.Diagnostics;

namespace XenoGears.Traits.Dumpable
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class DumpFormatAttribute : Attribute
    {
        public String NullObjectFormat { get; set; }
        public String DefaultExtension { get; set; }
    }
}