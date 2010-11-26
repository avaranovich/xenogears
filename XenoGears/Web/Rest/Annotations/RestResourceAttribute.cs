using System;
using System.Diagnostics;
using XenoGears.Reflection.Attributes.Snippets;
using XenoGears.Reflection.Attributes.Weight;

namespace XenoGears.Web.Rest.Annotations
{
    [SnippetAnnotationAnnotation]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    [DebuggerNonUserCode]
    public class RestResourceAttribute : WeightedAttribute
    {
        public String Uri { get; set; }
        public RestMethods Allow { get; set; }
        public bool SkipAuthentication { get; set; }
        public bool SkipAuthorization { get; set; }
    }
}
