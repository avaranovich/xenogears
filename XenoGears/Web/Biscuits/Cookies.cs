using System;
using System.Web;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Biscuits
{
    public class Cookies : OrderedDictionary<String, Cookie>
    {
        public Cookies()
        {
            throw new NotImplementedException();
        }

        public Cookies(HttpCookieCollection cookies)
        {
            throw new NotImplementedException();
        }

        public static implicit operator HttpCookieCollection(Cookies cookies)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Cookies(HttpCookieCollection cookies)
        {
            throw new NotImplementedException();
        }

        public void Export(HttpCookieCollection cookies)
        {
            throw new NotImplementedException();
        }
    }
}