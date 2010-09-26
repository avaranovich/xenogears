using System;
using XenoGears.Assertions;
using XenoGears.Formats.Annotations.Adapters;
using XenoGears.Functional;
using XenoGears.Reflection.Generics;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Formats.Adapters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SerializationCallbacksAttribute : TypeAdapterAttribute
    {
        public override Object AfterDeserialize(Type t, Object value)
        {
            if (value != null)
            {
                var m_afterdeserialize = t.GetMethods(BF.AllInstance).AssertSingle(m =>
                    !m.IsStatic && m.Name == "AfterDeserialize" && Seq.Equal(m.Params(), Type.EmptyTypes) && m.Ret() == typeof(void));
                m_afterdeserialize.Invoke(value, Type.EmptyTypes);
            }

            return value;
        }

        public override Object BeforeSerialize(Type t, Object value)
        {
            if (value != null)
            {
                var m_beforeserialize = t.GetMethods(BF.AllInstance).AssertSingle(m =>
                    !m.IsStatic && m.Name == "BeforeSerialize" && Seq.Equal(m.Params(), Type.EmptyTypes) && m.Ret() == typeof(void));
                m_beforeserialize.Invoke(value, Type.EmptyTypes);
            }

            return value;
        }
    }
}