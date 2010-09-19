using System;
using System.Reflection;

namespace XenoGears.Formats.Engines
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class PropertyEngineAttribute : Attribute
    {
        public abstract Object Deserialize(PropertyInfo pi, Json json);
        public abstract Json Serialize(PropertyInfo pi, Object value);
    }
}