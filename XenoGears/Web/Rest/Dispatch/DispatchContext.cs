using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Logging;
using XenoGears.Web.Rest.Annotations;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes.Snippets;
using XenoGears.Web.Rest.Context;
using Log = XenoGears.Web.Logging.Log;

namespace XenoGears.Web.Rest.Dispatch
{
    [DebuggerNonUserCode]
    public class DispatchContext
    {
        public DispatchResult Result { get; private set; }
        public BindContext BestMatch { get; private set; }
        public ReadOnlyCollection<BindContext> BindResults { get; private set; }
        private LevelLogger Debug { get { return Log.Dispatch.Debug; } }
        private LevelLogger Info { get { return Log.Dispatch.Info; } }
        private LevelLogger Error { get { return Log.Dispatch.Error; } }

        public RestRequest Request { get { return BestMatch == null ? null : BestMatch.Request; } }
        public String Resource { get { return BestMatch == null ? null : BestMatch.Resource; } }
        public String Method { get { return BestMatch == null ? null : BestMatch.Method; } }

        public ReadOnlyDictionary<String, String> ParsedResource { get { return BestMatch == null ? null : BestMatch.ParsedResource; } }
        public HandlerContext HandlerContext { get { return BestMatch == null ? null : BestMatch.HandlerContext; } }
        public MethodInfo HandlerCode { get { return BestMatch == null ? null : BestMatch.HandlerCode; } }
        public Action Handler { get { return BestMatch == null ? null : BestMatch.Handler; } }

        public DispatchContext(RestRequest req)
        {
            Info.EnsureBlankLine();
            Info.Write("Dispatching request ");
            var snippets = SnippetRegistry.Methods<RestResourceAttribute>();
            Info.WriteLine("(across {0} handlers)...", snippets.Count());
            BindResults = snippets.Select(snippet => new BindContext(req, snippet)).ToReadOnly();

            // todo. maybe invent some novel way to do dispatch
            // so that "http://localhost/daas/ldab/Users?$filter=substringof('Bu', Name)"
            // doesn't get dispatched to "/{relativeFilePath}" and cause a crash with 400
            // but rather becomes a 404 because it's just "/daas/ldap/{s_dataQuery}" with a typo
            var bestMatch = PickBestMatch(BindResults);
            if (bestMatch == null)
            {
                if (BindResults.Any(r => r.Result == BindResult.UrlMismatch)) Result = DispatchResult.UrlMismatch;
                if (BindResults.Any(r => r.Result == BindResult.ArgsMismatch)) Result = DispatchResult.ArgsMismatch;
                if (BindResults.Any(r => r.Result == BindResult.MethodMismatch)) Result = DispatchResult.MethodNotAllowed;
                if (BindResults.Any(r => r.Result == BindResult.NotAuthenticated)) Result = DispatchResult.NotAuthenticated;
                if (BindResults.Any(r => r.Result == BindResult.NotAuthorized)) Result = DispatchResult.NotAuthorized;
                if (BindResults.Count(r => r.Result == BindResult.Success) > 1) Result = DispatchResult.Ambiguous;
            } 
            else
            {
                BestMatch = bestMatch;
                Result = (DispatchResult)Enum.Parse(typeof(DispatchResult), bestMatch.Result.ToString());
            }

            Info.EnsureBlankLine();
            Info.WriteLine("Dispatch summary: {0}", Result);
            Info.WriteLine(BindResults.Select(r => "    * " + r.ToString()).StringJoin(Environment.NewLine));

            Info.EnsureBlankLine();
            if (Result != DispatchResult.Success) Error.WriteLine("Dispatch has failed with status code {0}", Result.ToHttpStatusCode());
            else Info.WriteLine("Bound handler: {0}", BestMatch.HandlerContext);
        }

        private static BindContext PickBestMatch(ReadOnlyCollection<BindContext> results)
        {
            // also pick not authenticated since subsequent reauthentication might lead to success
            var wannabes = results.Where(r => r.Result == BindResult.Success || r.Result == BindResult.NotAuthenticated).ToList();

            var dirty = true;
            while (dirty && results.Count() > 1)
            {
                var pairs = wannabes.SelectMany(r1 => wannabes.Select(r2 => Tuple.Create(r1, r2)));
                var resolvableConflict = pairs.FirstOrDefault(p =>
                {
                    var rp_prefix = p.Item1.ResourceTemplate.IndexOf("{") == -1 ? p.Item1.ResourceTemplate :
                        p.Item1.ResourceTemplate.Substring(0, p.Item1.ResourceTemplate.IndexOf("{"));
                    var rc_prefix = p.Item2.ResourceTemplate.IndexOf("{") == -1 ? p.Item2.ResourceTemplate :
                        p.Item2.ResourceTemplate.Substring(0, p.Item2.ResourceTemplate.IndexOf("{"));
                    return p.Item1 != p.Item2 && (rp_prefix.IsEmpty() || rc_prefix.StartsWith(rp_prefix));
                });

                if (resolvableConflict != null)
                {
                    wannabes.Remove(resolvableConflict.Item1);
                    dirty = true;
                }
                else
                {
                    dirty = false;
                }
            }

            return wannabes.Count() == 1 ? wannabes.Single() : null;
        }
    }
}