using System.Collections.ObjectModel;

namespace System
{
    // todo. or even better, inherit ICollection and implement it similarly to ROC<Object>
    // upd. hmm are you really sure that tuple should be foreachable?

    // todo 2. and finally write a tuple generator at least for 1-7 cases

    public interface ITuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        ReadOnlyCollection<Object> Items { get; }
        IMutableTuple ToMutable();
    }
}