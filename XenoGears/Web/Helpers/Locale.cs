using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace XenoGears.Web.Helpers
{
    [DebuggerNonUserCode]
    public static class Locale
    {
        public static CultureInfo Current
        {
            get
            {
                var req = HttpContext.Current.Request;
                var lang = req.UserLanguages == null ? null : req.UserLanguages.FirstOrDefault();
                if (lang == null) return null;
                return CultureInfo.GetCultureInfo(lang);
            }
        }
    }
}