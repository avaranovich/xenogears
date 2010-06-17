namespace XenoGears.Traits.Equivatable
{
    public interface IEquivatable<T>
        where T : IEquivatable<T>
    {
        bool Equiv(T other);
        int EquivHashCode();
    }
}