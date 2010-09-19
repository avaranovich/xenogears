using System;
using XenoGears.Web.Helpers;
using XenoGears.Functional;

namespace XenoGears.Web.Rest.Context
{
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