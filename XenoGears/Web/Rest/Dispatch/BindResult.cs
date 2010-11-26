namespace XenoGears.Web.Rest.Dispatch
{
    public enum BindResult
    {
        Unknown = 0,
        UrlMismatch = 1,
        ArgsMismatch = 2,
        MethodMismatch = 3,
        NotAuthenticated = 4,
        NotAuthorized = 5,
        Success = 6,
    }
}