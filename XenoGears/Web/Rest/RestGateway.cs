using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using XenoGears.Formats;
using XenoGears.Functional;
using XenoGears.Logging;
using XenoGears.Web.Helpers;
using XenoGears.Web.Rest.Context;
using XenoGears.Web.Rest.Dispatch;
using XenoGears.Assertions;
using Log = XenoGears.Web.Logging.Log;

namespace XenoGears.Web.Rest
{
    [DebuggerNonUserCode]
    public class RestGateway : IHttpHandler
    {
        public bool IsReusable { get { return false; } }
        private LevelLogger Debug { get { return Log.Rest.Debug; } }
        private LevelLogger Info { get { return Log.Rest.Info; } }
        private LevelLogger Error { get { return Log.Rest.Error; } }

        public void ProcessRequest(HttpContext ctx)
        {
            var rctx = new RestContext(ctx);
            var req = rctx.Request;
            var resp = rctx.Response;

            try
            {
                Info.EnsureBlankLine();
                Info.WriteLine("Received {0} request to {1}", req.Method, req.Resource);
                Info.WriteLine("    * Query: {0}", req.Query.Select(kvp => String.Format("{0} = {1}", kvp.Key, (String)kvp.Value)).StringJoin().Fluent(s => s.IsEmpty() ? "<empty>" : s));
                Info.WriteLine("    * Data: {0}", ((Json)req.Data).ToCompactString().Fluent(s => s.IsEmpty() ? "<empty>" : s));

                try
                {
                    var result = req.Dispatch();
                    if (result.Result == DispatchResult.Success)
                    {
                        result.Handler();
                    }
                    else
                    {
                        var statusCode = result.Result == DispatchResult.UrlMismatch ? HttpStatusCode.NotFound :
                            result.Result == DispatchResult.ArgsMismatch ? HttpStatusCode.NotAcceptable :
                            result.Result == DispatchResult.MethodNotAllowed ? HttpStatusCode.MethodNotAllowed :
                            result.Result == DispatchResult.NotAuthenticated ? HttpStatusCode.Unauthorized :
                            result.Result == DispatchResult.NotAuthorized ? HttpStatusCode.Forbidden :
                            result.Result == DispatchResult.Ambiguous ? HttpStatusCode.Ambiguous :
                            ((Func<HttpStatusCode>)(() => { throw AssertionHelper.Fail(); }))();
                        resp.Fail(statusCode);
                    }
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Error.EnsureBlankLine();
                    Error.WriteLine(CallStack.Enabled ? ex.ToString() : ex.Message);
                    resp.Fail(ex);
                }
            }
            catch (Exception)
            {
                resp.Fail("A fatal error has occurred");
            }
        }
    }

}