using System;

namespace XenoGears.Web.Rest.Annotations
{
    [Flags]
    public enum RestMethods
    {
        Get = 1,
        ReadOnly = Get,
        Put = 2,
        Create = Put,
        Post = 4,
        Merge = 8,
        Update = Post | Merge,
        Delete = 16,
        ReadWrite = Get | Create | Update | Delete,
    }
}