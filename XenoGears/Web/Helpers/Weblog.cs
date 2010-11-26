using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using XenoGears.Assertions;
using XenoGears.Logging;

namespace XenoGears.Web.Helpers
{
    [DebuggerNonUserCode]
    public static class Weblog
    {
        private const String key_logger = "XenoGears.Web.Helpers.Weblog::Logger";
        private const String key_medium = "XenoGears.Web.Helpers.Weblog::Medium";

        private static void EnsureMediumAndLogger()
        {
            var ctx = HttpContext.Current;
            if (!ctx.Items.Contains(key_logger))
            {
                var medium = new StringBuilder();
                ctx.Items[key_medium] = medium;

                var id = Guid.NewGuid();
                var logger = Logger.Get(String.Format("{0}+{1}", key_logger, id));
                logger.OverrideWriter(medium);
                ctx.Items[key_logger] = logger;
            }
        }

        public static Logger Current
        {
            get
            {
                EnsureMediumAndLogger();
                var ctx = HttpContext.Current;
                return ctx.Items[key_logger].AssertCast<Logger>();
            }
        }

        public static String Message
        {
            get
            {
                EnsureMediumAndLogger();
                var ctx = HttpContext.Current;
                var medium = ctx.Items[key_medium].AssertCast<StringBuilder>();
                return medium.ToString().Trim();
            }
        }
    }
}