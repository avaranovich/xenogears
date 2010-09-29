using System;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Adapters;
using XenoGears.Formats.Annotations.Engines;
using XenoGears.Formats.Annotations.Validators;
using XenoGears.Formats.Engines;
using XenoGears.Reflection;
using XenoGears.Reflection.Attributes;

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
            var t = mi is Type ? mi : (mi is PropertyInfo ? ((PropertyInfo)mi).PropertyType : ((Func<Type>)(() => { throw AssertionHelper.Fail(); }))());

            var prop_engine = pi.AttrOrNull<PropertyEngine>();
            var type_engine = t.AttrOrNull<TypeEngine>();
            var engine = prop_engine ?? (Engine)type_engine ?? new DefaultEngine();
            var value = engine.Deserialize(mi, this);

            var type_adapter = t.AttrOrNull<TypeAdapter>();
            if (type_adapter != null) value = type_adapter.BeforeSerialize(t, value);
            var prop_adapter = pi.AttrOrNull<PropertyAdapter>();
            if (prop_adapter != null) value = prop_adapter.BeforeSerialize(pi, value);

            var type_validator = t.AttrOrNull<TypeValidator>();
            if (type_validator != null) type_validator.Validate(t, value);
            var prop_validator = pi.AttrOrNull<PropertyValidator>();
            if (prop_validator != null) prop_validator.Validate(pi, value);

            return value;
        }
    }
}
