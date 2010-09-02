using System;
using System.Diagnostics;
using System.Linq;
using XenoGears.Assertions;
using XenoGears.Functional;
using XenoGears.Reflection.Shortcuts;

namespace XenoGears.Reflection.Simple
{
    [DebuggerNonUserCode]
    public static class SimpleApi
    {
        public static Object Get(this Object target, String name)
        {
            return target.Get(name, null);
        }

        public static Object GetOrDefault(this Object target, String name, Object @default)
        {
            return target.GetOrDefault(name, null, () => @default);
        }

        public static Object GetOrDefault(this Object target, String name, Func<Object> @default)
        {
            return target.GetOrDefault(name, null, @default);
        }

        public static Object GetOrDefault(this Object target, String name, Func<Object, Object> @default)
        {
            return target.GetOrDefault(name, null, () => @default(target));
        }

        public static Object Get(this Object target, String name, Type t)
        {
            return target.GetImpl(name, t, null);
        }

        public static Object GetOrDefault(this Object target, String name, Type t, Object @default)
        {
            return target.GetImpl(name, t, () => @default);
        }

        public static Object GetOrDefault(this Object target, String name, Type t, Func<Object> @default)
        {
            return target.GetImpl(name, t, @default);
        }

        public static Object GetOrDefault(this Object target, String name, Type t, Func<Object, Object> @default)
        {
            return target.GetImpl(name, t, () => @default(target));
        }

        public static Object Get<T>(this T target, String name)
        {
            return target.GetImpl(name, typeof(T), null);
        }

        public static Object GetOrDefault<T>(this T target, String name, Object @default)
        {
            return target.GetImpl(name, typeof(T), () => @default);
        }

        public static Object GetOrDefault<T>(this T target, String name, Func<Object> @default)
        {
            return target.GetImpl(name, typeof(T), @default);
        }

        public static Object GetOrDefault<T>(this T target, String name, Func<T, Object> @default)
        {
            return target.GetImpl(name, typeof(T), () => @default(target));
        }

        private static Object GetImpl(this Object target, String name, Type t, Func<Object> @default)
        {
            target.AssertNotNull();
            t = t ?? target.GetType();

            var f = t.GetField(name, BF.All);
            var p = t.GetProperties(BF.All).Where(pi => pi.Name == name && pi.GetIndexParameters().IsEmpty()).SingleOrDefault();
            (f != null && p != null).AssertFalse();

            if (f == null && p == null)
            {
                // try to find private slots in base classes
                var private_fs = t.Hierarchy().Select(bt => bt.GetField(name, BF.All));
                var private_ps = t.Hierarchy().Select(bt => bt.GetProperties(BF.All).Where(pi => pi.Name == name && pi.GetIndexParameters().IsEmpty()).SingleOrDefault2());

                f = private_fs.SingleOrDefault2(private_f => private_f != null);
                p = private_ps.SingleOrDefault2(private_f => private_f != null);
                (f != null && p != null).AssertFalse();

                if (f == null && p == null)
                {
                    // if this doesn't help - we give up
                    if (@default == null)
                    {
                        throw AssertionHelper.Fail();
                    }
                    else
                    {
                        return @default();
                    }
                }
            }

            if (f != null)
            {
                return f.GetValue(target);
            }
            else
            {
                return p.GetValue(target, null);
            }
        }

        public static void Set(this Object target, String name, Object value)
        {
            target.Set(name, value, null);
        }

        public static void Set(this Object target, String name, Object value, Type t)
        {
            target.AssertNotNull();
            t = t ?? target.GetType();

            var f = t.GetField(name, BF.All);
            var p = t.GetProperties(BF.All).Where(pi => pi.Name == name && pi.GetIndexParameters().IsEmpty()).SingleOrDefault();
            (f != null && p != null).AssertFalse();

            if (f == null && p == null)
            {
                // try to find private slots in base classes
                var private_fs = t.Hierarchy().Select(bt => bt.GetField(name, BF.All));
                var private_ps = t.Hierarchy().Select(bt => bt.GetProperties(BF.All).Where(pi => pi.Name == name && pi.GetIndexParameters().IsEmpty()).SingleOrDefault2());

                f = private_fs.SingleOrDefault2(private_f => private_f != null);
                p = private_ps.SingleOrDefault2(private_f => private_f != null);
                (f != null && p != null).AssertFalse();

                if (f == null && p == null)
                {
                    // if this doesn't help - we give up
                    throw AssertionHelper.Fail();
                }
            }

            if (f != null)
            {
                f.SetValue(target, value);
            }
            else
            {
                p.SetValue(target, value, null);
            }
        }
    }
}
