using System;
using System.Reflection;
using XenoGears.Assertions;
using XenoGears.Reflection.Attributes.Weight;

namespace XenoGears.Reflection.Attributes.Snippets
{
    public class Snippet<T> : IComparable<Snippet<T>>
        where T : MemberInfo
    {
        public SnippetAnnotationAnnotationAttribute AnnotationMetadata { get; private set; }
        public Attribute AssemblyAnnotation { get; private set; }
        public Attribute TypeAnnotation { get; private set; }
        public Attribute MemberAnnotation { get; private set; }
        public double Weight { get; private set; }
        public double Metric { get { return 1 / Weight; } }
        public T Member { get; private set; }

        public Snippet(
            SnippetAnnotationAnnotationAttribute annotationMetadata,
            Attribute assemblyAnnotation, 
            Attribute typeAnnotation, 
            Attribute methodAnnotation, 
            T code)
        {
            AnnotationMetadata = annotationMetadata;
            AssemblyAnnotation = assemblyAnnotation;
            TypeAnnotation = typeAnnotation;
            MemberAnnotation = methodAnnotation;
            Member = code;

            double acc;
            if (AnnotationMetadata.WeightAccumulationIsAdditive)
            {
                acc = 0.0;
                if (AssemblyAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)AssemblyAnnotation).Weight;
                if (TypeAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)TypeAnnotation).Weight;
                if (MemberAnnotation is WeightedAttribute)
                    acc += ((WeightedAttribute)MemberAnnotation).Weight;
            }
            else if (AnnotationMetadata.WeightAccumulationIsMultiplicative)
            {
                acc = 1.0;
                if (AssemblyAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)AssemblyAnnotation).Weight;
                if (TypeAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)TypeAnnotation).Weight;
                if (MemberAnnotation is WeightedAttribute)
                    acc *= ((WeightedAttribute)MemberAnnotation).Weight;
            }
            else
            {
                throw AssertionHelper.Fail();
            }

            Weight = acc;
        }

        public int CompareTo(Snippet<T> other)
        {
            if (other == null) return -1;
            if (this.AnnotationMetadata != other.AnnotationMetadata) return 0;
            return Weight.CompareTo(other.Weight);
        }
    }
}
