using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public abstract class Config
    {
        public MemberInfo Member { get; private set; }
        public Type Type { get { return Member as Type; } }
        public PropertyInfo Property { get { return Member as PropertyInfo; } }

        public Dictionary<Object, Object> Hash { get; private set; }
        public Object Get(String key) { return Hash[key]; }
        public Config Get(String key, out Object value) { value = Hash[key]; return this; }
        public Config Add(String key, Object value) { Hash.Add(key, value); return this; }
        public Config Put(String key, Object value) { Hash[key] = value; return this; }
        public Config Remove(String key) { Hash.Remove(key); return this; }

        protected Config(MemberInfo member)
        {
            Member = member;
        }

        protected Config(Type type)
        {
            Member = type;
        }

        protected Config(PropertyInfo property)
        {
            Member = property;
        }
    }
}