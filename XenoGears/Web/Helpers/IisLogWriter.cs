using System;
using System.Text;
using System.Web;
using XenoGears.Assertions;
using XenoGears.Strings.Writers;

namespace XenoGears.Web.Helpers
{
    public class IisLogWriter : BaseWriter
    {
        private HttpContext Ctx { get; set; }
        public IisLogWriter(HttpContext ctx) { Ctx = ctx; }

        public override Encoding Encoding { get { return Encoding.UTF8; } }
        protected override void CoreWrite(String s)
        {
            var ctx = HttpContext.Current.AssertNotNull();
            ctx.Response.AppendToLog(s);
        }
    }
}