using System;
using System.Diagnostics;

namespace XenoGears.Exceptions
{
    [DebuggerNonUserCode]
    public class DeferredException : Exception
    {
        public DeferredException(Exception deferred)
            : base(null, deferred)
        {
        }
    }
}