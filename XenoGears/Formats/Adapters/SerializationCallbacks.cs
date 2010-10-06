using System;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Formats.Adapters.Core;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Formats.Adapters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class SerializationCallbacks : TypeAdapter
    {
        public override Object AfterDeserialize(Type t, Object value)
        {
            if (value != null)
            {
                var m_afterdeserialize = t.GetMethods(BF.AllInstance).AssertSingleOrDefault(m =>
                    !m.IsStatic && m.Name == "AfterDeserialize" && Seq.Equal(m.Params(), Type.EmptyTypes) && m.Ret() == typeof(void));
                if (m_afterdeserialize != null) m_afterdeserialize.Invoke(value, Type.EmptyTypes);
            }

            return value;
        }

        public override Object BeforeSerialize(Type t, Object value)
        {
            if (value != null)
            {
                var m_beforeserialize = t.GetMethods(BF.AllInstance).AssertSingleOrDefault(m =>
                    !m.IsStatic && m.Name == "BeforeSerialize" && Seq.Equal(m.Params(), Type.EmptyTypes) && m.Ret() == typeof(void));
                if (m_beforeserialize != null) m_beforeserialize.Invoke(value, Type.EmptyTypes);
            }

            return value;
        }
    }
}