using System;
using System.Reflection;
using XenoGears.Formats.Annotations.Adapters;
using XenoGears.Formats.Annotations.Engines;
using XenoGears.Formats.Annotations.Validators;
using XenoGears.Formats.Engines;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Formats
{
    public partial class Json
    {
        public static String Serialize(Object value)
        {
            return new Json(value).ToString();
        }

        public static String Serialize(Type t, Object value)
        {
            return new Json(value).ToString();
        }

        public static String Serialize(MemberInfo mi, Object value)
        {
            return new Json(value).ToString();
        }

        public Json()
        {
        }

        public Json(Object value)
            : this(value == null ? null : value.GetType(), value)
        {
        }

        public Json(Type t, Object value)
            : this((MemberInfo)t, value)
        {
        }

        public Json(PropertyInfo pi, Object value)
            : this((MemberInfo)pi, value)
        {
        }

        public Json(MemberInfo mi, Object value)
        {
            mi = mi ?? (value == null ? null : value.GetType());
            var pi = mi as PropertyInfo;
            var t = mi is Type ? mi : (value == null ? null : value.GetType());

            var type_validator = t.AttrOrNull<TypeValidatorAttribute>();
            if (type_validator != null) type_validator.Validate(t, value);
            var prop_validator = pi.AttrOrNull<PropertyValidatorAttribute>();
            if (prop_validator != null) prop_validator.Validate(pi, value);

            var type_adapter = t.AttrOrNull<TypeAdapterAttribute>();
            if (type_adapter != null) value = type_adapter.BeforeSerialize(t, value);
            var prop_adapter = pi.AttrOrNull<PropertyAdapterAttribute>();
            if (prop_adapter != null) value = prop_adapter.BeforeSerialize(pi, value);

            var prop_engine = pi.AttrOrNull<PropertyEngineAttribute>();
            var type_engine = t.AttrOrNull<TypeEngineAttribute>();
            var engine = prop_engine ?? (EngineAttribute)type_engine ?? new DefaultEngineAttribute();
            _wrappee = engine.Serialize(mi, value);
        }
    }
}