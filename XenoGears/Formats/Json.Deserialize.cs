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
            return Deserialize((MemberInfo)t);
        }

        public Object Deserialize(PropertyInfo pi)
        {
            return Deserialize((MemberInfo)pi);
        }

        public Object Deserialize(MemberInfo mi)
        {
            mi = mi ?? ((Func<MemberInfo>)(() => { throw AssertionHelper.Fail(); }))();
            var pi = mi as PropertyInfo;
            var t = mi is Type ? mi : (mi is PropertyInfo ? ((PropertyInfo)mi).PropertyType : ((Func<Type>)(() => { throw AssertionHelper.Fail(); }))());

            var prop_engine = pi.AttrOrNull<PropertyEngineAttribute>();
            var type_engine = t.AttrOrNull<TypeEngineAttribute>();
            var engine = prop_engine ?? (EngineAttribute)type_engine ?? new DefaultEngineAttribute();
            var value = engine.Deserialize(mi, this);

            var type_adapter = t.AttrOrNull<TypeAdapterAttribute>();
            if (type_adapter != null) value = type_adapter.BeforeSerialize(t, value);
            var prop_adapter = pi.AttrOrNull<PropertyAdapterAttribute>();
            if (prop_adapter != null) value = prop_adapter.BeforeSerialize(pi, value);

            var type_validator = t.AttrOrNull<TypeValidatorAttribute>();
            if (type_validator != null) type_validator.Validate(t, value);
            var prop_validator = pi.AttrOrNull<PropertyValidatorAttribute>();
            if (prop_validator != null) prop_validator.Validate(pi, value);

            return value;
        }
    }
}
