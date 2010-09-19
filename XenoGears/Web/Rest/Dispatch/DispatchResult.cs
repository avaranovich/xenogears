namespace XenoGears.Web.Rest.Dispatch
{
    public enum DispatchResult
    {
        Unknown = 0,
        UrlMismatch = 1,
        ArgsMismatch = 2,
        MethodNotAllowed = 3,
        NotAuthenticated = 4,
        NotAuthorized = 5, 
        Ambiguous = 6,
        Success = 7, 
    }
}