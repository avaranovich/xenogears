using System.Collections;
using System.Text;

namespace System
{
    internal interface ITupleImpl
    {
        int GetHashCode(IEqualityComparer comparer);
        string ToString(StringBuilder sb);

        int Size { get; }
    }
}