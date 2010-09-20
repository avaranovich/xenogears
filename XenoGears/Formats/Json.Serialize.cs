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
        public static dynamic Serialize(Object value)
        {
            return new Json(value);
        }

        public static dynamic Serialize(Object value, Type descriptor)
        {
            return new Json(value, descriptor);
        }

        public static dynamic Serialize(Object value, PropertyInfo descriptor)
        {
            return new Json(value, descriptor);
        }

        public static dynamic Serialize(Object value, MemberInfo descriptor)
        {
            return new Json(value, descriptor);
        }

        protected Json()
        {
        }

        public Json(Object value)
            : this(value, value == null ? null : value.GetType())
        {
        }

        public Json(Object value, Type descriptor)
            : this(value, (MemberInfo)descriptor)
        {
        }

        public Json(Object value, PropertyInfo descriptor)
            : this(value, (MemberInfo)descriptor)
        {
        }

        public Json(Object value, MemberInfo descriptor)
        {
            var mi = descriptor ?? (value == null ? null : value.GetType());
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