using System;
using System.Diagnostics;
using System.Web;
using XenoGears.Formats;
using XenoGears.Web.Biscuits;
using XenoGears.Web.Headers;
using XenoGears.Web.Urls;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Streams;
using XenoGears.Assertions;

namespace XenoGears.Web.Rest.Context
{
    [DebuggerNonUserCode]
    public class RestRequest
    {
        private readonly RestContext _ctx;
        private HttpRequest _req { get { return _ctx.Native.Request; } }
        public RestContext Context { get { return _ctx; } }
        public RestHints Hints { get { return _ctx.Hints; } }
        public RestResponse Response { get { return _ctx.Response; } }
        public HttpRequest Native { get { return _req; } }
        public static RestRequest Current { get { return RestContext.Current == null ? null : RestContext.Current.Request; } }
        public RestRequest(RestContext ctx) { _ctx = ctx.AssertNotNull(); Url = new Url(Native.Url); }

        public Url Url { get; private set; }
        public String Resource { get { return Url.Resource; } }
        public String Method { get { var overridenMethod = _req.Headers["X-HTTP-Method"]; if (overridenMethod.IsNullOrEmpty()) overridenMethod = null; return overridenMethod ?? _req.HttpMethod; } }

        public dynamic this[String key]
        {
            get
            {
                if (key.IsNullOrEmpty()) return null;
                var is_header = Char.IsUpper(key[0]);
                return is_header ? Headers[key] : (Json.ParseOrDefault(Query[key]) ?? new Json(Query[key]));
            }
        }

        public RequestHeaders Headers
        {
            get { return new RequestHeaders(Native); }
        }

        public ReadOnlyDictionary<String, dynamic> Query
        {
            get
            {
//                var query = new Dictionary<String, String>();
//
                // 1) query string
//                query.AddElements(Url.Query);
//
                // 2) form
//                var ctype = new ContentType(Native.ContentType);
//                if (ctype == "application/x-www-form-urlencoded")
//                {
                    // todo. take encoding into account
//                    var s_requestData = Native.InputStream.DumpToString();
//                    query.AddElements(new Query(s_requestData));
//                }
//
                // 3) cookies
//                var cookies = new Cookies(Native.Cookies);
//                cookies.Keys.ForEach(key => query.Add(key, cookies[key].Value));

                // todo. include parsed resource as well!!!
//
//                return query.ToReadOnly();

                throw new NotImplementedException();
            }
        }

        public Cookies Cookies
        {
            get { return new Cookies(Native.Cookies); }
        }

        // todo. null data => {} json, but not null!
        public dynamic Data
        {
            get
            {
                // todo. take encoding into account
                var data = Native.InputStream.DumpToString();
                if (data.IsNullOrEmpty()) return null;

                var is_json = Native.ContentType == "application/json";
                return is_json ? Json.Parse(data) : new Json(data);
            }
        }
    }
}