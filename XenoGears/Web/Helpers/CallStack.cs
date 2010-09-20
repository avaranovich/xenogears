﻿using System.Web;

namespace XenoGears.Web.Helpers
{
    public static class CallStack
    {
        public static bool Enabled
        {
            get
            {
                var debugEnabled = HttpContext.Current.IsDebuggingEnabled;
                // todo. this creates a potential security hole!
                // note. though I had to do this since commented line ain't work
//                var customErrorsDisabled = !HttpContext.Current.IsCustomErrorEnabled;
#if DEBUG
                var customErrorsDisabled = true;
#else
                    var customErrorsDisabled = false;
#endif

                return debugEnabled && customErrorsDisabled;
            }
        }
    }
}