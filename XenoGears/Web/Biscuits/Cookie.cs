using System;
using System.Web;

namespace XenoGears.Web.Biscuits
{
    public class Cookie
    {
        public String Value { get { throw new NotImplementedException(); } }
        public DateTime Expires { get { throw new NotImplementedException(); } }

        public Cookie()
        {
            throw new NotImplementedException();
        }

        public Cookie(HttpCookie cookie)
        {
            throw new NotImplementedException();
        }

        public static implicit operator HttpCookie(Cookie cookie)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Cookie(HttpCookie cookie)
        {
            throw new NotImplementedException();
        }
    }
}