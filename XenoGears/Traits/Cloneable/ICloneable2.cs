using System;

namespace XenoGears.Traits.Cloneable
{
    public interface ICloneable2
    {
        Guid UniqueId { get; }
        Guid ProtoId { get; }

        T ShallowClone<T>();
        T DeepClone<T>();
    }
}