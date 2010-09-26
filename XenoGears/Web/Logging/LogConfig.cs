using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Config.Codebase;
using XenoGears.Config;
using XenoGears.Logging;

namespace XenoGears.Web.Logging
{
    [Config("~/config/projects")]
    [DebuggerNonUserCode]
    public class LogConfig : Dictionary<String, Level>
    {
        private LogConfig() { }
        public static LogConfig Current { get { return AppConfig.Get<LogConfig>(); } }
    }
}