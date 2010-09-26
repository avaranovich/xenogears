using System;
using System.Diagnostics;
using System.Web;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Biscuits
{
    [DebuggerNonUserCode]
    public class Cookies : OrderedDictionary<String, dynamic>
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