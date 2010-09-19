using System;
using System.Web;

namespace XenoGears.Web.Biscuits
{
    public static class CookieHelper
    {
        public static void Import(this HttpResponse resp, Cookies cookies)
        {
            throw new NotImplementedException();
        }

        public static void Import(this HttpCookieCollection http_cookies, Cookies cookies)
        {
            throw new NotImplementedException();
        }
    }
}