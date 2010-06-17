using System;
using System.Diagnostics;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class EnumHelper
    {
        public static bool TryParse<E>(String s, out E e)
        {
            try
            {
                e = (E)Enum.Parse(typeof(E), s);
                return true;
            }
            catch (Exception)
            {
                e = default(E);
                return false;
            }
        }
    }
}
