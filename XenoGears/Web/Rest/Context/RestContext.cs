using System.Diagnostics;
using System.Web;

namespace XenoGears.Web.Rest.Context
{
    [DebuggerNonUserCode]
    public class RestContext
    {
        public static RestContext Current
        {
            get
            {
                var hctx = HttpContext.Current;
                if (hctx == null) return null;
                if (!hctx.Items.Contains("XenoGears.RestContext")) return null;
                return (RestContext)hctx.Items["XenoGears.RestContext"];
            }
        }

        private readonly HttpContext _native;
        public HttpContext Native { get { return _native; } }

        private readonly RestRequest _req;
        public RestRequest Request { get { return _req; } }

        private readonly RestResponse _resp;
        public RestResponse Response { get { return _resp; } }

        private readonly RestHints _hints;
        public RestHints Hints { get { return _hints; } }

        public RestContext(HttpContext native)
        {
            _native = native;
            _req = new RestRequest(this);
            _resp = new RestResponse(this);
            _hints = new RestHints(this);
            native.Items["XenoGears.RestContext"] = this;
        }
    }
}