using System;
using System.Reflection;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Engines;
using XenoGears.Functional;
using XenoGears.Reflection;
using XenoGears.Formats.Configuration;
using XenoGears.Assertions;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static T Deserialize<T>(String json)
        {
            return (T)Deserialize(json, typeof(T));
        }

        public static T Deserialize<T>(String json, T pattern)
        {
            return (T)Deserialize(json, typeof(T));
        }

        public static Object Deserialize(String json, Type descriptor)
        {
            if (json == null) { descriptor.IsValueType.AssertFalse(); return null; }
            return Parse(json).Deserialize(descriptor);
        }

        public static Object Deserialize(String json, PropertyInfo descriptor)
        {
            if (json == null) { descriptor.SlotType().IsValueType.AssertFalse(); return null; }
            return Parse(json).Deserialize(descriptor);
        }

        public static Object Deserialize(String json, MemberInfo descriptor)
        {
            if (json == null) { descriptor.SlotType().IsValueType.AssertFalse(); return null; }
            return Parse(json).Deserialize(descriptor);
        }

        public T Deserialize<T>()
        {
            return (T)Deserialize(typeof(T));
        }

        public T Deserialize<T>(T pattern)
        {
            return (T)Deserialize(typeof(T));
        }

        public Object Deserialize(Type descriptor)
        {
            return Deserialize((MemberInfo)descriptor);
        }

        public Object Deserialize(PropertyInfo descriptor)
        {
            return Deserialize((MemberInfo)descriptor);
        }

        public Object Deserialize(MemberInfo descriptor)
        {
            var mi = descriptor ?? ((Func<MemberInfo>)(() => { throw AssertionHelper.Fail(); }))();
            var pi = mi as PropertyInfo;
            var t = mi is Type ? (Type)mi : (mi is PropertyInfo ? ((PropertyInfo)mi).PropertyType : ((Func<Type>)(() => { throw AssertionHelper.Fail(); }))());

            var engine = pi.Config().Engine ?? (Engine)new DefaultEngine();
            if (pi != null && engine is TypeEngine) mi = pi.PropertyType;
            var value = engine.Deserialize(mi, this);
            value = t.Config().Adapters.Fold(value, (curr, adapter) => adapter.AfterDeserialize(t, curr));
            value = pi.Config().Adapters.Fold(value, (curr, adapter) => adapter.AfterDeserialize(pi, curr));
            t.Config().Validators.ForEach(validator => validator.Validate(t, value));
            pi.Config().Validators.ForEach(validator => validator.Validate(pi, value));

            return value;
        }
    }
}
