using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using XenoGears.Collections.Dictionaries;

namespace XenoGears.Web.Headers
{
    public class ResponseHeaders : BaseDictionary<String, String>
    {
        public override int Count
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerator<KeyValuePair<String, String>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(String key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(String key, out String value)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override void Add(String key, String value)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(String key)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(String key, String value)
        {
            throw new NotImplementedException();
        }

        public String this[HttpResponseHeader header]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ResponseHeaders()
        {
            throw new NotImplementedException();
        }

        public ResponseHeaders(HttpResponse response)
        {
            throw new NotImplementedException();
        }

        public void Export(HttpResponse response)
        {
            throw new NotImplementedException();
        }
    }
}