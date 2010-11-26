using System.IO;

namespace XenoGears.Traits.Dumpable
{
    public interface IDumpableAsText
    {
        // should be implemented privately since all public operations are defined in IDumpableAsTextTrait
        // that's an important guideline to follow since extension methods are null-safe,
        // but what you could have implemented as a public instance method would be not
        void DumpAsText(TextWriter writer);
    }
}