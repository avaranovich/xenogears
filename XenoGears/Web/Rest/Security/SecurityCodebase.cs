using System;

namespace XenoGears.Web.Rest.Security
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SecurityCodebaseAttribute : Attribute
    {
    }
}
