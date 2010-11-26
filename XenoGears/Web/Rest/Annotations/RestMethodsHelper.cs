using System;
using System.Collections.Generic;
using System.Diagnostics;
using XenoGears.Assertions;
using XenoGears.Functional;

namespace XenoGears.Web.Rest.Annotations
{
    [DebuggerNonUserCode]
    internal static class RestMethodsHelper
    {
        public static String ToVerbsList(this RestMethods meths)
        {
            var verbs = new List<String>();
            if ((meths & RestMethods.Get) != 0) verbs.Add("GET");
            if ((meths & RestMethods.Put) != 0) verbs.Add("PUT");
            if ((meths & RestMethods.Post) != 0) verbs.Add("POST");
            if ((meths & RestMethods.Merge) != 0) verbs.Add("MERGE");
            if ((meths & RestMethods.Delete) != 0) verbs.Add("DELETE");

            ((meths | RestMethods.Get | RestMethods.Put | RestMethods.Post | RestMethods.Merge | RestMethods.Delete) ==
             (RestMethods.Get | RestMethods.Put | RestMethods.Post | RestMethods.Merge | RestMethods.Delete)).AssertTrue();
            return verbs.StringJoin();
        }
    }
}