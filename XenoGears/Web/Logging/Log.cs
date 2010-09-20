using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using XenoGears.Web.Helpers;
using XenoGears.Assertions;
using XenoGears.Logging;
using XenoGears.Functional;

namespace XenoGears.Web.Logging
{
    public class Log
    {
        private readonly StringBuilder _medium = new StringBuilder();
        private String _message { get { return _medium == null ? null : _medium.ToString().Trim(); } }
        public static String Message { get { return _instance._message; } }

        internal static Logger Rest { get { return Get("XenoGears.Web.Rest"); } }
        internal static Logger Dispatch { get { return Get("XenoGears.Web.Rest.Dispatch"); } }

        private Dictionary<String, Logger> _loggers = new Dictionary<String, Logger>();
        public static Logger Get(String name)
        {
            return _instance._loggers.GetOrCreate(name, () =>
            {
                // this is done in order to provide per-request logs
                var logger = Logger.Get(name + "_" + Guid.NewGuid().ToString().Replace("-", "_"));
                logger.OverrideWriter(_instance._medium);
                logger.Writer.Multiplex(new IisLogWriter(_instance._ctx));

                // todo. implement hot-swap of logger levels
                logger.MinLevel = LogConfig.Current.GetOrDefault(name.Replace(".", "_").ToLower(), Level.Debug);
                return logger;
            });
        }

        private readonly HttpContext _ctx;
        private Log(HttpContext ctx) { _ctx = ctx; }
        private static Log _instance
        {
            get
            {
                var ctx = HttpContext.Current;
                var storage = ctx.Items;
                var key = "XenoGears.Web";
                if (!storage.Contains(key)) storage[key] = new Log(ctx);
                return storage[key].AssertCast<Log>();
            }
        }
    }
}