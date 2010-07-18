namespace XenoGears.Traits.Freezable
{
    public interface IFreezable
    {
        bool IsFrozen { get; }

        bool CannotBeFrozen { get; }
        void Freeze();
        void FreezeForever();

        bool CannotBeUnfrozen { get; }
        void Unfreeze();
        void UnfreezeForever();
    }
}
