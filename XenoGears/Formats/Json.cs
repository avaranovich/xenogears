using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Reflection;

namespace XenoGears.Formats
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) accessing keys/values and such for primitives throws an exception immediately
    // 4) everything => to lower case, when serializing from c# to json
    // 5) tolerate trailing semicolons

    public class Json : BaseDictionary<String, dynamic>
    {
        // API

        public bool IsPrimitive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsComplex
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int Count
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerator<KeyValuePair<String, dynamic>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(String key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(String key, out dynamic value)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override void Add(String key, dynamic value)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(String key)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(String key, dynamic value)
        {
            throw new NotImplementedException();
        }

        // JSON => .NET

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

        public static Json Parse(String json)
        {
            // also understand inline comments!!
            throw new NotImplementedException();
        }

        public static Json ParseOrDefault(String json)
        {
            try { return Parse(json); }
            catch { return new Json(json); }
        }

        public static Json Load(String uri)
        {
            // get from url
            throw new NotImplementedException();
        }

        public static void Save(String uri, Json json)
        {
            // post to url
            throw new NotImplementedException();
        }

        // .NET -> JSON

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
            // 2) HttpCookie
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

        public String ToCompactString()
        {
            throw new NotImplementedException();
        }

        public String ToPrettyString()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ToCompactString();
        }
    }
}