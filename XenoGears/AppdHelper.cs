using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class AppdHelper
    {
        [DebuggerNonUserCode]
        private class AppdBox
        {
            public Object Data { get; set; }

            public AppdBox(Object data)
            {
                Data = data;
            }
        }

        [DebuggerNonUserCode]
        private class AppdBox<T> : AppdBox
        {
            public new T Data
            {
                get { return (T)base.Data; }
                set { base.Data = value; }
            }

            public AppdBox(T data)
                : base(data)
            {
            }
        }

        public static bool AppdContains(this String key)
        {
            return AppDomain.CurrentDomain.GetData(key).AssertCast<AppdBox>() != null;
        }

        public static T AppdGet<T>(this String key)
        {
            var box = AppDomain.CurrentDomain.GetData(key).AssertCast<AppdBox<T>>();
            return box.AssertNotNull().Data;
        }

        public static void AppdSet<T>(this String key, T value)
        {
            AppDomain.CurrentDomain.SetData(key, new AppdBox<T>(value));
        }

        public static void AppdRemove(this String key)
        {
            AppDomain.CurrentDomain.SetData(key, null);
        }

        public static V AppdGetOrDefault<V>(this String key)
        {
            return key.AppdGetOrDefault(default(V));
        }

        public static V AppdGetOrDefault<V>(this String key, V value)
        {
            return key.AppdGetOrDefault(() => value);
        }

        public static V AppdGetOrDefault<V>(this String key, Func<V> factory)
        {
            return key.AppdGetOrDefault(_ => factory());
        }

        public static V AppdGetOrDefault<V>(this String key, Func<String, V> factory)
        {
            return key.AppdContains() ? key.AppdGet<V>() : factory(key);
        }

        public static V AppdGetOrCreate<V>(this String key)
        {
            return key.AppdGetOrCreate(default(V));
        }

        public static V AppdGetOrCreate<V>(this String key, V value)
        {
            return key.AppdGetOrCreate(() => value);
        }

        public static V AppdGetOrCreate<V>(this String key, Func<V> factory)
        {
            return key.AppdGetOrCreate(_ => factory());
        }

        public static V AppdGetOrCreate<V>(this String key, Func<String, V> factory)
        {
            if (key.AppdContains())
            {
                return key.AppdGet<V>();
            }
            else
            {
                var created = factory(key);
                key.AppdSet(created);
                return created;
            }
        }
    }
}