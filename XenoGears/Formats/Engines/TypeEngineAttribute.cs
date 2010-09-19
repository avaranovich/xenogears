using System;

namespace XenoGears.Formats.Engines
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public abstract class TypeEngineAttribute : Attribute
    {
        public abstract Object Deserialize(Type t, Json json);
        public abstract Json Serialize(Type t, Object value);
    }
}