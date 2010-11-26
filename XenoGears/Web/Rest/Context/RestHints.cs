using System;
using System.Diagnostics;
using XenoGears.Functional;
using Debug = XenoGears.Web.Helpers.Debug;

namespace XenoGears.Web.Rest.Context
{
    [DebuggerNonUserCode]
    public class RestHints
    {
        public bool Prettyprint { get; private set; }
        public String jQueryCacheBypass { get; private set; }

        public RestHints(RestContext ctx)
        {
            var req = ctx.Request;

            Prettyprint = req.Query["$pretty"] != null && (req.Query["$pretty"] == "1" || req.Query["$pretty"].ToLower() == "true");
            Prettyprint |= (Debug.Enabled && req.Native.UserAgent.IsNeitherNullNorEmpty());
            req.Query.Remove("$pretty");

            jQueryCacheBypass = req.Query["_"];
            req.Query.Remove("_");
        }
    }
}