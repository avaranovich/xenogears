using System.Web;

namespace XenoGears.Web.Helpers
{
    public static class Debug
    {
        public static bool Enabled
        {
            get { return HttpContext.Current.IsDebuggingEnabled; }
        }
    }
}