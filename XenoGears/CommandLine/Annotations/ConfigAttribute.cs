using System;

namespace XenoGears.CommandLine.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ConfigAttribute : Attribute
    {
    }
}