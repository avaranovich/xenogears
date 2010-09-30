using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using XenoGears.Formats;
using XenoGears.Strings.Writers;
using XenoGears.Web.Biscuits;
using XenoGears.Web.Headers;
using XenoGears.Functional;
using XenoGears.Assertions;
using XenoGears.Web.Helpers;
using XenoGears.Strings;
using XenoGears.Web.Logging;
using Debug = XenoGears.Web.Helpers.Debug;

namespace XenoGears.Web.Rest.Context
{
    [DebuggerNonUserCode]
    public class RestResponse : BaseWriter
    {
        private readonly RestContext _ctx;
        private HttpRequest _req { get { return _ctx.Native.Request; } }
        private HttpResponse _resp { get { return _ctx.Native.Response; } }
        public RestContext Context { get { return _ctx; } }
        public RestHints Hints { get { return _ctx.Hints; } }
        public RestRequest Request { get { return _ctx.Request; } }
        public HttpResponse Native { get { return _resp; } }
        public static RestResponse Current { get { return RestContext.Current == null ? null : RestContext.Current.Response; } }
        public RestResponse(RestContext ctx) { _ctx = ctx.AssertNotNull(); }

        public HttpStatusCode StatusCode
        {
            get { return (HttpStatusCode)Native.StatusCode; }
            set { Native.StatusCode = (int)value; }
        }

        // todo. respect response encoding headers here!
        public override Encoding Encoding { get { return Encoding.UTF8; } }
        protected override void CoreWrite(String s) { Native.Write(s); }
        public void BinaryWrite(byte[] bytes) { Native.BinaryWrite(bytes); }
        public void StreamWrite(Stream stream) { stream.CopyTo(Native.OutputStream); }

        public dynamic this[String key]
        {
            get
            {
                if (key.IsNullOrEmpty()) return null;
                var is_header = Char.IsUpper(key[0]);
                var s_value = is_header ? Headers[key] : Cookies[key].Value;
                return Json.ParseOrDefault(s_value) ?? new Json(s_value);
            }
            set
            {
                var is_header = Char.IsUpper(key[0]);
                if (is_header) Headers[key] = value;
                else Cookies[key] = value;
            }
        }

        public ResponseHeaders Headers
        {
            // todo. auto-apply default encoding => UTF-8
            // todo. auto-apply default content type => text/html
            get { return new ResponseHeaders(Native); }
            set { throw new NotImplementedException(); }
        }

        public Cookies Cookies
        {
            get { return new Cookies(Native.Cookies); }
            set { throw new NotImplementedException(); }
        }

        public void Succeed(Object result = null)
        {
            StatusCode.IsSuccess().AssertTrue();
            Complete(result);
        }

        public void Succeed(HttpStatusCode statusCode, Object result = null)
        {
            StatusCode = statusCode;
            Succeed(result);
        }

        public void Fail(Object result = null)
        {
            if (StatusCode == HttpStatusCode.OK)
            {
                var is_exn = result is Exception;
                if (is_exn) StatusCode = HttpStatusCode.InternalServerError;
                else StatusCode = HttpStatusCode.BadRequest;
            }

            StatusCode.IsFail().AssertTrue();
            Complete(result);
        }

        public void Fail(HttpStatusCode statusCode, Object result = null)
        {
            StatusCode = statusCode;
            Fail(result);
        }

        // todo. when outputting HTML, also inject the "$(document).ready(function(){window.log.server(<actual log>});"
        private void Complete(Object result = null)
        {
            if (result == null) Native.End();

            var result_ex = result as Exception;
            if (result_ex != null)
            {
                Native.Clear();

                // note. why there's no line info in the stack trace?
                // see http://stackoverflow.com/questions/2673623/iis-not-giving-line-numbers-in-stack-trace-even-though-pdb-present
                var message = (CallStack.Enabled ? result_ex.ToString() : result_ex.Message).ToHtml();
                var trace = (Debug.Enabled ? Log.Message : null).ToHtml();

                var is_json = Native.ContentType == "application/json";
                if (is_json)
                {
                    this["Content-Type"] = "application/json";
                    var lowlevel_result = new Json(new { success = true, result = message, trace = trace });
                    Write(Hints.Prettyprint ? lowlevel_result.ToPrettyString() : lowlevel_result.ToCompactString());
                    Native.End();
                }
                else
                {
                    this["Content-Type"] = "text/html";
                    // note. trace will be written by Gateway.cs
                    // note. no need to dump exception, since it's already included into trace
                    Native.End();
                }
            }

            var result_json = result as Json;
            if (result_json != null)
            {
                (this.Text() == null || this.Text().IsEmpty()).AssertTrue();
                this["Content-Type"] = Hints.Prettyprint ? "text/html" : "application/json";
                var lowlevel_result = new Json(new { success = true, result = result_json, trace = (Debug.Enabled ? Log.Message : null).ToHtml() });
                Write(Hints.Prettyprint ? lowlevel_result.ToPrettyString() : lowlevel_result.ToCompactString());
                Native.End();
            }

            var result_string = result as String;
            if (result_string != null)
            {
                (this.Text() == null || this.Text().IsEmpty()).AssertTrue();
                Write(result_string);
                Native.End();
            }

            var result_bytes = result as byte[];
            if (result_bytes != null)
            {
                (this.Bytes() == null || this.Bytes().IsEmpty()).AssertTrue();
                BinaryWrite(result_bytes);
                Native.End();
            }

            var result_stream = result as Stream;
            if (result_stream != null)
            {
                (this.Bytes() == null || this.Bytes().IsEmpty()).AssertTrue();
                StreamWrite(result_stream);
                Native.End();
            }

            throw AssertionHelper.Fail();
        }
    }
}
