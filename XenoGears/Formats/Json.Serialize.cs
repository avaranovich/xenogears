using System;
using System.Reflection;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Engines;
using XenoGears.Functional;
using XenoGears.Formats.Configuration;
using XenoGears.Assertions;
using XenoGears.Reflection;

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

        internal Json()
        {
        }

        public Json(Object value)
            : this(value.IsJsonPrimitive() ? null : value, value == null || value.IsJsonPrimitive() ? null : value.GetType())
        {
            if (value.IsJsonPrimitive())
            {
                _my_primitive = value;
                _my_state = State.Primitive;
            }
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
            var t = mi is Type ? (Type)mi : (value == null ? null : value.GetType());

            pi.Config().Validators.ForEach(validator => validator.Validate(pi, value));
            t.Config().Validators.ForEach(validator => validator.Validate(t, value));
            value = pi.Config().Adapters.Fold(value, (curr, adapter) => adapter.BeforeSerialize(pi, curr));
            value = t.Config().Adapters.Fold(value, (curr, adapter) => adapter.BeforeSerialize(t, curr));

            if (value == null)
            {
                _my_state = State.Primitive;
                _my_primitive = null;
            }
            else if (value is Json)
            {
                _wrappee = value.AssertCast<Json>();
            }
            else
            {
                var engine = pi.Config().Engine ?? (Engine)new DefaultEngine();
                if (engine is TypeEngine && !(mi is Type)) mi = mi.Type();
                _wrappee = engine.Serialize(mi, value);
            }
        }
    }
}