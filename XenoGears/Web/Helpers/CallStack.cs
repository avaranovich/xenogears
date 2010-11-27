using System.Diagnostics;
using System.Web;

namespace XenoGears.Web.Helpers
{
    [DebuggerNonUserCode]
    public static class CallStack
    {
        public static bool Enabled
        {
            get
            {
                var debugEnabled = HttpContext.Current.IsDebuggingEnabled;
                // todo. this creates a potential security hole!
                // note. though I had to do this since commented line ain't work
//                var customErrorsDisabled = !HttpContext.Current.IsCustomErrorEnabled;
                var customErrorsDisabled = Debug.Enabled;

                return debugEnabled && customErrorsDisabled;
            }
        }

        public static bool Disabled
        {
            get { return !Enabled; }
        }
    }
}