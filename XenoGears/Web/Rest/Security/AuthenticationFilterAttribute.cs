using System;
using XenoGears.Reflection.Attributes.Snippets;

namespace XenoGears.Web.Rest.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [SnippetAnnotationAnnotation(AssemblyMarker = typeof(SecurityCodebaseAttribute), TypeMarker = typeof(SecurityCodebaseAttribute))]
    public class AuthenticationFilterAttribute : Attribute
    {
    }
}