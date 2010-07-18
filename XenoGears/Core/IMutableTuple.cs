using System.Collections.Generic;

namespace System
{
    public interface IMutableTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        IList<Object> Items { get; }
        ITuple ToImmutable();
    }
}