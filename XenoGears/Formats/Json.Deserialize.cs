using System;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection;

namespace XenoGears.Formats
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 4) deserialization is case-insensitive

    public partial class Json
    {
        public static T Deserialize<T>(String json)
        {
            return (T)Deserialize(typeof(T), json);
        }

        public static Object Deserialize(Type t, String json)
        {
            if (json == null) { t.IsValueType.AssertFalse(); return null; }
            return Parse(json).Deserialize(t);
        }

        public static Object Deserialize(MemberInfo mi, String json)
        {
            if (json == null) { mi.SlotType().IsValueType.AssertFalse(); return null; }
            return Parse(json).Deserialize(mi);
        }

        public T Deserialize<T>()
        {
            return (T)Deserialize(typeof(T));
        }

        public Object Deserialize(Type t)
        {
            throw new NotImplementedException();
        }

        public Object Deserialize(MemberInfo mi)
        {
            // todo. respect propertyinfo's adapters, validators and blah blah
            throw new NotImplementedException();
        }
    }
}
