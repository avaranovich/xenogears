using System;
using System.Diagnostics;
using System.Net;
using XenoGears.Assertions;

namespace XenoGears.Web.Rest.Dispatch
{
    [DebuggerNonUserCode]
    public static class StatusCodeHelper
    {
        public static HttpStatusCode ToHttpStatusCode(this BindResult result)
        {
            switch (result)
            {
                case BindResult.UrlMismatch:
                    return HttpStatusCode.NotFound;
                case BindResult.ArgsMismatch:
                    return HttpStatusCode.NotAcceptable;
                case BindResult.MethodMismatch:
                    return HttpStatusCode.MethodNotAllowed;
                case BindResult.NotAuthenticated:
                    return HttpStatusCode.Unauthorized;
                case BindResult.NotAuthorized:
                    return HttpStatusCode.Forbidden;
                case BindResult.Success:
                    return HttpStatusCode.OK;
                default:
                    throw AssertionHelper.Fail();
            }
        }

        public static HttpStatusCode ToHttpStatusCode(this DispatchResult result)
        {
            switch (result)
            {
                case DispatchResult.UrlMismatch:
                    return HttpStatusCode.NotFound;
                case DispatchResult.ArgsMismatch:
                    return HttpStatusCode.NotAcceptable;
                case DispatchResult.MethodNotAllowed:
                    return HttpStatusCode.MethodNotAllowed;
                case DispatchResult.NotAuthenticated:
                    return HttpStatusCode.Unauthorized;
                case DispatchResult.NotAuthorized:
                    return HttpStatusCode.Forbidden;
                case DispatchResult.Ambiguous:
                    return HttpStatusCode.Ambiguous;
                case DispatchResult.Success:
                    return HttpStatusCode.OK;
                default:
                    throw AssertionHelper.Fail();
            }
        }
    }
}