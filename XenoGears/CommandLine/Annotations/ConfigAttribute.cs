using System;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ConfigAttribute : Attribute
    {
        public String Name { get; set; }

        public ConfigAttribute() {}
        public ConfigAttribute(String name) { Name = name; }
    }
}