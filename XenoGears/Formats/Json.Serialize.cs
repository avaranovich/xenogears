using System;
using System.Reflection;

namespace XenoGears.Formats
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 4) everything => to lower case, when serializing from c# to json

    public partial class Json
    {
        public static String Serialize(Object o)
        {
            return new Json(o).ToString();
        }

        public static String Serialize(Type t, Object o)
        {
            return new Json(o).ToString();
        }

        public static String Serialize(MemberInfo mi, Object o)
        {
            return new Json(o).ToString();
        }

        public Json()
        {
            throw new NotImplementedException();
        }

        public Json(Object o)
        {
            // todo. special processing for:
            // 1) Json
            // 2) dynamic
            throw new NotImplementedException();
        }

        public Json(Type t, Object o)
        {
            throw new NotImplementedException();
        }

        public Json(MemberInfo mi, Object o)
        {
            // todo. respect propertyinfo's adapters, validators and blah blah
            throw new NotImplementedException();
        }


    }
}