using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using XenoGears.Assertions;
using XenoGears.Formats.Engines.Core;
using XenoGears.Formats.Configuration.Default.Annotations;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Attributes;
using XenoGears.Reflection;

namespace XenoGears.Formats.Engines
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class AdhocEngine : TypeEngine
    {
        public override Object Deserialize(Type t, Json json)
        {
            var m_deserialize = t.GetMethods(BF.All).AssertSingle(m => m.Name == "Deserialize" && (
                Seq.Equal(m.Params(), typeof(Json)) || 
                (Seq.Equal(m.Params(), typeof(Object)) && m.GetParameters()[0].HasAttr<DynamicAttribute>())));
            if (m_deserialize.IsStatic)
            {
                (m_deserialize.Ret() != typeof(void)).AssertTrue();
                return m_deserialize.Invoke(null, json.MkArray());
            }
            else
            {
                var a_json = t.AttrOrNull<JsonAttribute>();
                var default_ctor = a_json == null ? true : a_json.DefaultCtor;
                var instance = default_ctor ? t.CreateInstance() : t.CreateUninitialized();

                (m_deserialize.Ret() == typeof(void)).AssertTrue();
                m_deserialize.Invoke(instance, json.MkArray());
                return instance;
            }
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