using System;
using System.Diagnostics;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public class ShortcutAttribute : Attribute
    {
        public int Priority { get; set; }
        public String Schema { get; private set; }
        public String Description { get; set; }

        public ShortcutAttribute(String shortcut)
        {
            Schema = shortcut;
        }
    }
}