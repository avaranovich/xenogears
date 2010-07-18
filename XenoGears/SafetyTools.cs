using System;
using System.Diagnostics;

namespace XenoGears
{
    [DebuggerNonUserCode]
    public static class SafetyTools
    {
        public static void SafeDo(this Action @do)
        {
            try
            {
                @do();
            }
            catch (Exception)
            {
                return;
            }
        }

        public static T SafeEval<T>(this Func<T> eval)
        {
            return SafeEval(eval, default(T));
        }

        public static T SafeEval<T>(this Func<T> eval, T @default)
        {
            return SafeEval(eval, () => @default);
        }

        public static T SafeEval<T>(this Func<T> eval, Func<T> @default)
        {
            try
            {
                return eval();
            }
            catch (Exception)
            {
                return @default();
            }
        }

        public static String SafeToString(this Object obj)
        {
            return obj == null ? null : obj.ToString();
        }

        public static int SafeHashCode(this Object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}