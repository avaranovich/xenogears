using System.Diagnostics;

namespace XenoGears.Traits.Freezable
{
    [DebuggerNonUserCode]
    public static class IFreezableTrait
    {
        public static void Freeze<T>(this T freezable)
            where T : IFreezable
        {
            if (freezable != null) freezable.Freeze();
        }

        public static void Unfreeze<T>(this T freezable)
            where T : IFreezable
        {
            if (freezable != null) freezable.Unfreeze();
        }
    }
}