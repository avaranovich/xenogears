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

        public static Object GetOrDefault(this Object target, String name)
        {
            return target.GetOrDefault(name, null);
        }

        public static Object Get(this Object target, String name, Type t)
        {
            return target.GetImpl(name, t, true);
        }

        public static Object GetOrDefault(this Object target, String name, Type t)
        {
            return target.GetImpl(name, t, false);
        }

        private static Object GetImpl(this Object target, String name, Type t, bool throwIfNotFound)
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
                    if (throwIfNotFound)
                    {
                        throw AssertionHelper.Fail();
                    }
                    else
                    {
                        return null;
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
