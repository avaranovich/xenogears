using System.Net;

namespace XenoGears.Web.Helpers
{
    public static class StatusCodeHelper
    {
        public static bool IsSuccess(this HttpStatusCode code)
        {
            return (int)code < 400;
        }

        public static bool IsFail(this HttpStatusCode code)
        {
            return (int)code >= 400;
        }
    }
}