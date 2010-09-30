using System;
using System.Diagnostics;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Configuration;
using XenoGears.Formats.Configuration.Default;
using XenoGears.Assertions;
using XenoGears.Reflection;
using XenoGears.Strings;

namespace XenoGears.Formats.Engines
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) support interfaces => just create stubs for those
    // 4) deserialization is case-insensitive. understand this!
    // 5) don't forget to quote json strings for string literals and string-serializable

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class DefaultEngine : TypeEngine
    {
        public override Object Deserialize(Type t, Json json)
        {
            var cfg = t.Config().DefaultEngine().Config;
            if (cfg.IsPrimitive)
            {
                if (t.IsJsonPrimitive())
                {
                    return json._primitive;
                }
                else
                {
                    t.SupportsSerializationToString().AssertTrue();
                    var s_value = json._primitive.AssertCast<String>();
                    return s_value.FromInvariantString(t);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override Json Serialize(Type t, Object value)
        {
            if (value == null) return new Json(null);

            var cfg = t.Config().DefaultEngine().Config;
            if (cfg.IsPrimitive)
            {
                if (t.IsJsonPrimitive())
                {
                    return new JsonPrimitive(value);
                }
                else
                {
                    t.SupportsSerializationToString().AssertTrue();
                    return new JsonPrimitive(t.ToInvariantString());
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
