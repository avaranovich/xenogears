using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XenoGears.Functional;

namespace XenoGears.Traits.Disposable
{
    [DebuggerNonUserCode]
    internal class DisposableChain : DisposableAction, IEnumerable<DisposableAction>
    {
        private readonly List<DisposableAction> _flat = new List<DisposableAction>();
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<DisposableAction> GetEnumerator() { return _flat.GetEnumerator(); }

        public DisposableChain(params DisposableAction[] actions) : this((IEnumerable<DisposableAction>)actions) {}
        public DisposableChain(IEnumerable<DisposableAction> actions)
            : base(() => actions.Where(action => action != null).ForEach(action => action.Dispose()))
        {
            actions.ForEach(action =>
            {
                if (action == null) return;
                var chain = action as DisposableChain;
                if (chain != null) _flat.AddRange(chain);
                else _flat.Add(action);
            });
        }
    }
}