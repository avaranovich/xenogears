using System;
using System.Diagnostics;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection.Attributes.Weight;

namespace XenoGears.Reflection.Attributes.Snippets
{
    [DebuggerNonUserCode]
    public class Snippet : IComparable<Snippet>
    {
        public SnippetAnnotationAnnotationAttribute AnnotationMetadata { get; private set; }
        public Attribute AssemblyAnnotation { get; private set; }
        public Attribute TypeAnnotation { get; private set; }
        public Attribute MethodAnnotation { get; private set; }
        public double Weight { get; private set; }
        public double Metric { get; private set; }
        public MethodInfo Code { get; private set; }

        public Snippet(
            SnippetAnnotationAnnotationAttribute annotationMetadata,
            Attribute assemblyAnnotation, 
            Attribute typeAnnotation, 
            Attribute methodAnnotation, 
            MethodInfo code)
        {
            AnnotationMetadata = annotationMetadata;
            AssemblyAnnotation = assemblyAnnotation;
            TypeAnnotation = typeAnnotation;
            MethodAnnotation = methodAnnotation;
            Code = code;

            double acc;
            if (AnnotationMetadata.WeightAccumulationIsAdditive)
            {
                acc = 0.0;
                if (AssemblyAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)AssemblyAnnotation).Weight;
                if (TypeAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)TypeAnnotation).Weight;
                if (MethodAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)MethodAnnotation).Weight;
            }
            else if (AnnotationMetadata.WeightAccumulationIsMultiplicative)
            {
                acc = 1.0;
                if (AssemblyAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)AssemblyAnnotation).Weight;
                if (TypeAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)TypeAnnotation).Weight;
                if (MethodAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)MethodAnnotation).Weight;
            }
            else
            {
                throw AssertionHelper.Fail();
            }

            Weight = acc;
            Metric = 1 / acc;
        }

        public int CompareTo(Snippet other)
        {
            if (other == null) return -1;
            if (this.AnnotationMetadata != other.AnnotationMetadata) return 0;
            return Weight.CompareTo(other.Weight);
        }
    }
}
