using System.Diagnostics;
using System.Web;

namespace XenoGears.Web.Helpers
{
    [DebuggerNonUserCode]
    public static class Debug
    {
        public static bool Enabled
        {
            get { return HttpContext.Current.IsDebuggingEnabled; }
        }

        public static bool Disabled
        {
            get { return !Enabled; }
        }
    }
}