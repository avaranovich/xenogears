using System;
using System.Web;
using XenoGears.Strings;
using XenoGears.Web.Logging;
using XenoGears.Web.Rest;
using XenoGears.Functional;
using XenoGears.Web.Helpers;

namespace XenoGears.Web
{
    public class Gateway : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += (o, e) =>
            {
                var ctx = ((HttpApplication)o).Context;
                ctx.Response.HijackOutputStream();
                ctx.RemapHandler(new RestGateway());
            };

            app.PreSendRequestHeaders += (o, e) =>
            {
                var ctx = ((HttpApplication)o).Context;
                var req = ctx.Request;
                var resp = ctx.Response;

                // provide feedback to browser users
                // if the response is empty, we dump status and trace
                // so that user gets to know that something has happened
                if (ctx.IsDebuggingEnabled && req.UserAgent.IsNeitherNullNorEmpty())
                {
                    var text = resp.Text();
                    if (text != null && text.IsEmpty())
                    {
                        resp.Write(String.Format("{0} {1}", resp.StatusCode, resp.StatusDescription));
                        if (Debug.Enabled) resp.Write((Environment.NewLine + Environment.NewLine + Log.Message).ToHtml());
                    }
                }
            };
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}