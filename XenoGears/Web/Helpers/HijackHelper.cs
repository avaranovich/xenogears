using System;
using System.Web;
using XenoGears.Web.Rest.Context;
using XenoGears.Streams;
using XenoGears.Assertions;

namespace XenoGears.Web.Helpers
{
    public static class HijackHelper
    {
        public static void HijackOutputStream(this HttpResponse resp)
        {
            resp.Filter.AssertNull();
            resp.Filter = new InterceptorStream(resp.Filter);
        }

        public static String Text(this HttpResponse resp)
        {
            var interceptor = resp.Filter as InterceptorStream;
            return interceptor == null ? null : interceptor.ReadInterceptedText();
        }

        public static byte[] Bytes(this HttpResponse resp)
        {
            var interceptor = resp.Filter as InterceptorStream;
            return interceptor == null ? null : interceptor.ReadInterceptedBytes();
        }

        public static void HijackOutputStream(this RestResponse resp)
        {
            resp.Native.HijackOutputStream();
        }

        public static String Text(this RestResponse resp)
        {
            return resp.Native.Text();
        }

        public static byte[] Bytes(this RestResponse resp)
        {
            return resp.Native.Bytes();
        }
    }
}