using System;
using System.Diagnostics;
using System.Threading;
using XenoGears.Assertions;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class TlsHelper
    {
        [DebuggerNonUserCode]
        private class TlsBox
        {
            public Object Data { get; set; }

            public TlsBox(Object data)
            {
                Data = data;
            }
        }

        [DebuggerNonUserCode]
        private class TlsBox<T> : TlsBox
        {
            public new T Data
            { 
                get { return (T)base.Data; }
                set { base.Data = value; }
            }

            public TlsBox(T data)
                : base(data)
            {
            }
        }

        public static bool TlsContains(this String key)
        {
            var slot = Thread.GetNamedDataSlot(key);
            return Thread.GetData(slot).AssertCast<TlsBox>() != null;
        }

        public static T TlsGet<T>(this String key)
        {
            var slot = Thread.GetNamedDataSlot(key);
            var box = Thread.GetData(slot).AssertCast<TlsBox<T>>();
            return box.AssertNotNull().Data;
        }

        public static void TlsSet<T>(this String key, T value)
        {
            var slot = Thread.GetNamedDataSlot(key);
            Thread.SetData(slot, new TlsBox<T>(value));
        }

        public static void TlsRemove(this String key)
        {
            Thread.FreeNamedDataSlot(key);
        }

        public static V TlsGetOrDefault<V>(this String key)
        {
            return key.TlsGetOrDefault(default(V));
        }

        public static V TlsGetOrDefault<V>(this String key, V value)
        {
            return key.TlsGetOrDefault(() => value);
        }

        public static V TlsGetOrDefault<V>(this String key, Func<V> factory)
        {
            return key.TlsGetOrDefault(_ => factory());
        }

        public static V TlsGetOrDefault<V>(this String key, Func<String, V> factory)
        {
            return key.TlsContains() ? key.TlsGet<V>() : factory(key);
        }

        public static V TlsGetOrCreate<V>(this String key)
        {
            return key.TlsGetOrCreate(default(V));
        }

        public static V TlsGetOrCreate<V>(this String key, V value)
        {
            return key.TlsGetOrCreate(() => value);
        }

        public static V TlsGetOrCreate<V>(this String key, Func<V> factory)
        {
            return key.TlsGetOrCreate(_ => factory());
        }

        public static V TlsGetOrCreate<V>(this String key, Func<String, V> factory)
        {
            if (key.TlsContains())
            {
                return key.TlsGet<V>();
            }
            else
            {
                var created = factory(key);
                key.TlsSet(created);
                return created;
            }
        }
    }
}
