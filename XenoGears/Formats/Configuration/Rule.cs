using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;

namespace XenoGears.Formats.Configuration
{
    [DebuggerNonUserCode]
    public abstract class Rule
    {
        private Func<MemberInfo, bool> Filter { get; set; }
        public bool AppliesTo(MemberInfo member) { return member != null && Filter(member); }

        public bool IsAdhoc { get; private set; }
        public bool IsPermanent { get { return !IsAdhoc; } }

        public Dictionary<Object, Object> Hash { get; private set; }
        public Object Get(String key) { return Hash[key]; }
        public Rule Get(String key, out Object value) { value = Hash[key]; return this; }
        public Rule Add(String key, Object value) { Hash.Add(key, value); return this; }
        public Rule Put(String key, Object value) { Hash[key] = value; return this; }
        public Rule Remove(String key) { Hash.Remove(key); return this; }

        protected Rule(Func<MemberInfo, bool> member, bool isAdhoc)
        {
            Filter = member.AssertNotNull();
            IsAdhoc = isAdhoc;
            Hash = new Dictionary<Object, Object>();
        }

        protected Rule(Func<Type, bool> type, bool isAdhoc)
        {
            type.AssertNotNull();
            Filter = mi => mi is Type && type(mi.AssertCast<Type>());
            IsAdhoc = isAdhoc;
            Hash = new Dictionary<Object, Object>();
        }

        protected Rule(Func<PropertyInfo, bool> property, bool isAdhoc)
        {
            property.AssertNotNull();
            Filter = mi => mi is PropertyInfo && property(mi.AssertCast<PropertyInfo>());
            IsAdhoc = isAdhoc;
            Hash = new Dictionary<Object, Object>();
        }
    }
}