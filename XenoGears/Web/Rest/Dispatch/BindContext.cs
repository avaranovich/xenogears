using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XenoGears.Formats;
using XenoGears.Logging;
using XenoGears.Web.Rest.Annotations;
using XenoGears.Web.Rest.Context;
using XenoGears.Web.Rest.Security;
using XenoGears.Web.Urls;
using XenoGears.Assertions;
using XenoGears.Collections.Dictionaries;
using XenoGears.Functional;
using XenoGears.Reflection.Attributes.Snippets;
using Log = XenoGears.Web.Logging.Log;
using XenoGears.Strings;

namespace XenoGears.Web.Rest.Dispatch
{
    [DebuggerNonUserCode]
    public class BindContext
    {
        public RestRequest Request { get; private set; }
        public BindResult Result { get; private set; }
        private LevelLogger Debug { get { return Log.Dispatch.Debug; } }
        private LevelLogger Info { get { return Log.Dispatch.Info; } }
        private LevelLogger Error { get { return Log.Dispatch.Error; } }

        public Snippet<MethodInfo> Snippet { get; private set; }
        public double Weight { get { return Snippet.Weight; } }
        private RestResourceAttribute AsmResource { get { return Snippet.AssemblyAnnotation.AssertCast<RestResourceAttribute>(); } }
        private RestResourceAttribute TypeResource { get { return Snippet.TypeAnnotation.AssertCast<RestResourceAttribute>(); } }
        private RestResourceAttribute MethodResource { get { return Snippet.MemberAnnotation.AssertCast<RestResourceAttribute>(); } }

        public String ResourceTemplate { get; private set; }
        public String Resource { get; private set; }
        public ReadOnlyDictionary<String, String> ParsedResource { get; private set; }

        public RestMethods AllowedMethods { get; private set; }
        public String Method { get; private set; }

        public HandlerContext HandlerContext { get; private set; }
        public MethodInfo HandlerCode { get; private set; }
        public Action Handler { get; private set; }

        public BindContext(RestRequest request, Snippet<MethodInfo> snippet)
        {
            Request = request.AssertNotNull();
            Snippet = snippet.AssertNotNull();
            Debug.EnsureBlankLine();
            Debug.WriteLine("Binding request to {0}...", snippet.Member.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));

            var asmUri = AsmResource.Uri ?? String.Empty;
            var typeUri = TypeResource.Uri ?? String.Empty;
            var methodUri = MethodResource.Uri ?? String.Empty;
            var uriTemplate = asmUri + typeUri + methodUri;
            ResourceTemplate = uriTemplate;
            Debug.WriteLine("    * Resource template: {0}", ResourceTemplate);

            var parsed1 = new Url(request.Resource).Parse(uriTemplate);
            var parsed2 = new Url(request.Resource + "/").Parse(uriTemplate);
            var parsed = parsed1 ?? parsed2;
            Resource = request.Resource;
            ParsedResource = parsed;
            Debug.WriteLine("    * Resource: {0}", Resource);

            if (parsed != null)
            {
                Debug.WriteLine("    + Resource matches the template");

                var code = snippet.Member;
                HandlerCode = code;
                HandlerContext = new HandlerContext(request, parsed, code);
                Handler = () =>
                {
                    HandlerContext.Validate().AssertTrue();
                    var result = HandlerContext.Invoke();

                    var response = request.Response;
                    var is_json = request["Content-Type"] == "application/json";
                    if (is_json) response.Succeed(new Json(result));
                    else response.Succeed(result);
                };

                if (HandlerContext.Validate())
                {
                    Debug.WriteLine("    + Handler arguments are bound successfully");

                    var asmMethods = AsmResource.Allow;
                    var typeMethods = TypeResource.Allow;
                    var methodMethods = MethodResource.Allow;
                    var allowedMethods = asmMethods & typeMethods & methodMethods;
                    AllowedMethods = allowedMethods;
                    Debug.WriteLine("    * Allowed HTTP methods: " + AllowedMethods);

                    var allowMethod = false;
                    allowMethod |= (request.Method == "GET" && ((allowedMethods & RestMethods.Get) != 0));
                    allowMethod |= (request.Method == "PUT" && ((allowedMethods & RestMethods.Put) != 0));
                    allowMethod |= (request.Method == "POST" && ((allowedMethods & RestMethods.Post) != 0));
                    allowMethod |= (request.Method == "MERGE" && ((allowedMethods & RestMethods.Merge) != 0));
                    allowMethod |= (request.Method == "DELETE" && ((allowedMethods & RestMethods.Delete) != 0));
                    Method = request.Method;
                    Debug.WriteLine("    * HTTP method: " + Method);

                    if (allowMethod)
                    {
                        Debug.WriteLine("    + HTTP method is allowed");

                        var authenticators = SnippetRegistry.Methods<AuthenticationFilterAttribute>();
                        var skipAuthentication = AsmResource.SkipAuthentication || TypeResource.SkipAuthentication || MethodResource.SkipAuthentication;
                        var boundAuthenticators = authenticators.Select(authenticator =>
                        {
                            Debug.WriteLine("    * Authorizing against {0}...", authenticator.Member.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
                            return new HandlerContext(request, parsed, authenticator.Member);
                        }).ToReadOnly();
                        var authenticated = skipAuthentication || boundAuthenticators.All(authenticator => authenticator.Validate() && Equals(authenticator.Invoke(), true));

                        if (authenticated)
                        {
                            Debug.WriteLine("    + Authentication successful");

                            var authorizers = SnippetRegistry.Methods<AuthorizationFilterAttribute>();
                            var skipAuthorization = AsmResource.SkipAuthorization || TypeResource.SkipAuthorization || MethodResource.SkipAuthorization;
                            var boundAuthorizers = authorizers.Select(authorizer =>
                            {
                                Debug.WriteLine("    * Authorizing against {0}...", authorizer.Member.GetCSharpRef(ToCSharpOptions.InformativeWithNamespaces));
                                return new HandlerContext(request, parsed, authorizer.Member);
                            }).ToReadOnly();
                            var authorized = skipAuthorization || boundAuthorizers.All(authorizer => authorizer.Validate() && Equals(authorizer.Invoke(), true));

                            if (authorized)
                            {
                                Debug.WriteLine("    + Authorization successful");
                                Debug.WriteLine("    * Bind successful");
                                Result = BindResult.Success;
                            }
                            else
                            {
                                Debug.WriteLine("    FAIL => Failed to authorize");
                                Result = BindResult.NotAuthorized;
                            }
                        }
                        else
                        {
                            Debug.WriteLine("    FAIL => Failed to authenticate");
                            Result = BindResult.NotAuthenticated;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("    FAIL => HTTP method is not allowed");
                        Result = BindResult.MethodMismatch;
                    }
                }
                else
                {
                    Debug.WriteLine("    FAIL => Failed to bind handler arguments");
                    Result = BindResult.ArgsMismatch;
                }
            }
            else
            {
                Debug.WriteLine("FAIL => Resource doesn't match the template");
                Result = BindResult.UrlMismatch;
            }
        }

        public override String ToString()
        {
            var allowed = new List<String>();
            if ((AllowedMethods & RestMethods.Get) != 0) allowed.Add("GET");
            if ((AllowedMethods & RestMethods.Put) != 0) allowed.Add("PUT");
            if ((AllowedMethods & RestMethods.Post) != 0) allowed.Add("POST");
            if ((AllowedMethods & RestMethods.Merge) != 0) allowed.Add("MERGE");
            if ((AllowedMethods & RestMethods.Delete) != 0) allowed.Add("DELETE");
            return String.Format("{0} {1} => {2} (w = {3})", ResourceTemplate, 
                allowed.IsEmpty() ? "N/A" : allowed.StringJoin(), Result, Snippet.Weight);
        }
    }
}