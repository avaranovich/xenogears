using System;
using System.Diagnostics;
using System.Reflection;

namespace XenoGears.Reflection
{
    [DebuggerNonUserCode]
    public static class DelegateHelper
    {
        // note. this method is used by Jink!
        public static T BindDelegate<T>(this MethodBase m)
        {
            throw new NotImplementedException();
        }
    }
}