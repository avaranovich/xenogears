using System;

namespace XenoGears.Formats.Engines.Configuration.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class JsonExcludeAttribute : Attribute
    {
    }
}
