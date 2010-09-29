using System;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Engines;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Generics;

namespace XenoGears.Formats.Engines
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class AdhocEngine : TypeEngine
    {
        public override Object Deserialize(Type t, Json json)
        {
            var m_deserialize = t.GetMethods(BF.AllStatic).AssertSingle(m =>
                m.IsStatic && m.Name == "Deserialize" && Seq.Equal(m.Params(), typeof(Json)) && m.Ret() == typeof(Object));
            return m_deserialize.Invoke(null, json.MkArray());
        }

        public override Json Serialize(Type t, Object value)
        {
            if (value == null) return new Json("null");
            var m_serialize = t.GetMethods(BF.AllInstance).AssertSingle(m =>
                !m.IsStatic && m.Name == "Serialize" && Seq.Equal(m.Params(), Type.EmptyTypes) && m.Ret() == typeof(Json));
            return m_serialize.Invoke(value, Type.EmptyTypes).AssertCast<Json>();
        }
    }
}