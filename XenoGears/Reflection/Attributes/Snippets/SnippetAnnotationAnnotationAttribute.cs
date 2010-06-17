using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Attributes.Snippets
{
    // note. I've hesitated to name this class with this weird name, but choose to leave it
    // that's because I think this name helps to understand the purpose of the attribute
    // it's an Attribute that Annotates attributes that Annotate snippets of code

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [DebuggerNonUserCode]
    public class SnippetAnnotationAnnotationAttribute : Attribute
    {
        public Type AssemblyMarker { get; set; }
        public Type TypeMarker { get; set; }
        // MethodMarker is an attribute that is annotated with this attribute
        // see SnippetsRegistry.For<A>
//        public Type MethodMarker { get; set; }
        public bool AutoIncludeAllMethodsInMarkedClasses { get; set; }

        private enum WeightAccumulation { Additive, Multiplicative }
        private WeightAccumulation _weightAccumulation;

        public bool WeightAccumulationIsMultiplicative
        {
            get { return _weightAccumulation == WeightAccumulation.Multiplicative; }
            set
            {
                value.AssertTrue();
                _weightAccumulation = WeightAccumulation.Multiplicative;
            }
        }

        public bool WeightAccumulationIsAdditive
        {
            get { return _weightAccumulation == WeightAccumulation.Additive; }
            set
            {
                value.AssertTrue();
                _weightAccumulation = WeightAccumulation.Additive;
            }
        }
    }
}