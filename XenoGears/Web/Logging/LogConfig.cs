using System;
using System.Collections.Generic;
using XenoGears.Config.Codebase;
using XenoGears.Config;
using XenoGears.Logging;

namespace XenoGears.Web.Logging
{
    [Config("~/config/projects")]
    public class LogConfig : Dictionary<String, Level>
    {
        private LogConfig() { }
        public static LogConfig Current { get { return AppConfig.Get<LogConfig>(); } }
    }
}