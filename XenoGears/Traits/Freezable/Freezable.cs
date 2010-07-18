using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Traits.Freezable
{
    [DebuggerNonUserCode]
    public class Freezable : IFreezable
    {
        public bool IsFrozen { get; private set; }
        public bool CannotBeFrozen { get; private set; }
        public bool CannotBeUnfrozen { get; private set; }

        public void Freeze()
        {
            CannotBeFrozen.AssertFalse();
            if (!IsFrozen)
            {
                OnFreezing(); 
                IsFrozen = true;
            }
        }

        public void FreezeForever()
        {
            Freeze();
            CannotBeUnfrozen = true;
        }

        public void Unfreeze()
        {
            CannotBeUnfrozen.AssertFalse();
            if (IsFrozen)
            {
                OnUnfreezing();
                IsFrozen = false;
            }
        }

        public void UnfreezeForever()
        {
            Unfreeze();
            CannotBeFrozen = true;
        }

        protected virtual void OnFreezing() { }
        protected virtual void OnUnfreezing() { }
    }
}