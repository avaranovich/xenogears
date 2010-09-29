using System;
using System.Collections.Generic;
using System.Reflection;
using XenoGears.Formats.Annotations.Engines;
using XenoGears.Functional;

namespace XenoGears.Formats.Configuration
{
    public class Config
    {
        public MemberInfo Member { get; private set; }
        public Type Type { get { return Member as Type; } }
        public PropertyInfo Property { get { return Member as PropertyInfo; } }

        public bool IsBanned { get { return (bool)Hash.GetOrDefault("IsBanned"); } set { Hash["IsBanned"] = value; } }
        public Config Ban() { IsBanned = true; return this; }
        public Config Unban() { IsBanned = false; return this; }

        public Engine Engine { get { return (Engine)Hash.GetOrDefault("Engine"); } set { Hash["Engine"] = value; } }
        public Config SetEngine(Engine engine) { Engine = engine; return this; }
        public Config ResetEngine() { Engine = null; return this; }

        public Dictionary<Object, Object> Hash { get; private set; }
        public Object Get(String key) { return Hash[key]; }
        public Config Get(String key, out Object value) { value = Hash[key]; return this; }
        public Config Add(String key, Object value) { Hash.Add(key, value); return this; }
        public Config Put(String key, Object value) { Hash[key] = value; return this; }
        public Config Remove(String key) { Hash.Remove(key); return this; }

        public Config(MemberInfo member)
        {
            Member = member;
        }

        public Config(Type type)
        {
            Member = type;
        }

        public Config(PropertyInfo property)
        {
            Member = property;
        }
    }
}