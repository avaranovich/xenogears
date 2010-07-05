using System;
using System.Diagnostics;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [DebuggerNonUserCode]
    public class ShortcutAttribute : Attribute
    {
        public int Priority { get; set; }
        public String Shortcut { get; private set; }
        public String Description { get; set; }

        public ShortcutAttribute(String shortcut)
        {
            Shortcut = shortcut;
        }
    }
}