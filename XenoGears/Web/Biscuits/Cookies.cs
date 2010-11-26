using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Biscuits
{
    [DebuggerNonUserCode]
    public class Cookies : BaseDictionary<String, dynamic>
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

        public override int Count
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(string key, out dynamic value)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override void Add(string key, dynamic value)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(string key, dynamic value)
        {
            throw new NotImplementedException();
        }
    }
}