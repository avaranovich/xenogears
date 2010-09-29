using System;
using System.Diagnostics;
using XenoGears.Formats.Annotations.Engines;
using XenoGears.Assertions;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Strings;
using XenoGears.Reflection;

namespace XenoGears.Formats.Engines
{
    // todo.
    // 1) respect [DefaultValue]
    // 2) serialize/deserialize dictionaries as { key : value }
    // 3) support interfaces => just create stubs for those
    // 4) deserialization is case-insensitive

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class DefaultSerializationAttribute : TypeEngineAttribute
    {
        public override Object Deserialize(Type t, Json json)
        {
            throw new NotImplementedException();
        }

        public override Json Serialize(Type t, Object value)
        {
//            if (t == null || value == null)
//            {
//                (t == null && value == null).AssertTrue();
//                return new Json{_my_primitive = null, _my_state = Json.State.Primitive};
//            }
//            else if (t == typeof(string) || t == typeof(bool) || t == typeof(float) || t == typeof(double) ||
//                t == typeof(sbyte) || t == typeof(short) || t == typeof(int) || t == typeof(long) ||
//                t == typeof(byte) || t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong))
//            {
//                return new Json{_my_primitive = value, _my_state = Json.State.Primitive};
//            }
//            else if (t.SupportsSerializationToString())
//            {
//                var s_value = value.ToInvariantString();
//                s_value = String.Format("{0}'{1}", t.GetCSharpRef(new ToCSharpOptions()), s_value);
//                return new Json{_my_primitive = s_value, _my_state = Json.State.Primitive};
//            }
//            else
//            {
//                var list_el = t.GetEnumerableElement();
//                if (list_el != null)
//                {
//                    throw new NotImplementedException();
//                }
//
//                var dict_el = t.GetDictionaryElement();
//                if (dict_el != null)
//                {
//                    var k_el = dict_el.Value.Key;
//                    var v_el = dict_el.Value.Value;
//                    throw new NotImplementedException();
//                }
//
//                var props = t.GetProperties(BF.AllInstance);
//            }

            throw new NotImplementedException();
        }
    }
}
