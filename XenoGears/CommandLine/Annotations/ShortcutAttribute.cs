using System;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
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