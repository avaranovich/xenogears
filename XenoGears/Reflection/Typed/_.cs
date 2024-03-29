using System;
using System.Diagnostics;

namespace XenoGears.Reflection.Typed
{
    [DebuggerNonUserCode]
    internal class _<T>
    {
        public T Value { get; private set; }

        public _(T value)
        {
            Value = value;
        }

        public static implicit operator _<T>(T unwrapped)
        {
            return new _<T>(unwrapped);
        }

        public static implicit operator T(_<T> wrapped)
        {
            return wrapped.Value;
        }

        public override string ToString()
        {
            return String.Format("'{0}' in box of '{1}'",
                Value == null ? "null" : Value.ToString(), typeof(T));
        }
    }

    [DebuggerNonUserCode]
    internal class _ : _<Object>
    {
        public _(Object value)
            : base(value)
        {
        }
    }
}