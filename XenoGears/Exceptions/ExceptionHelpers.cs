using System;
using System.Diagnostics;
using XenoGears.Reflection.Shortcuts;
using XenoGears.Assertions;

namespace XenoGears.Exceptions
{
    [DebuggerNonUserCode]
    public static class ExceptionHelpers
    {
        public static void PreserveStackTrace(this Exception exn)
        {
            // note. thanks to StackOverflow and to Anton Tykhyy
            // http://stackoverflow.com/questions/1009762/how-can-i-rethrow-an-inner-exception-while-maintaining-the-stack-trace-generated

            if (exn == null) return;
            var m_InternalPreserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BF.All);
            m_InternalPreserveStackTrace.AssertNotNull();
            m_InternalPreserveStackTrace.Invoke(exn, null);
        }

        public static void EraseStackTrace(this Exception exn)
        {
            if (exn == null) return;
            var f_stackTrace = typeof(Exception).GetField("_stackTrace", BF.PrivateInstance);
            var f_stackTraceString = typeof(Exception).GetField("_stackTraceString", BF.PrivateInstance);
            f_stackTrace.SetValue(exn, null);
            f_stackTraceString.SetValue(exn, null);
        }
    }
}