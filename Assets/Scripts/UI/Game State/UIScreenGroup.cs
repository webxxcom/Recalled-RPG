using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class UIScreenGroup : ToggleableGroup
    {
        [SerializeField] GameStateSO _baseGameState;
        readonly Stack<IToggleable> _screens = new();

        protected override bool AddActiveToggleable(IToggleable screen)
        {
            IToggleable prev = Active.Count != 0 ? Active[0] : null;

            if (base.AddActiveToggleable(screen) && prev != null)
                _screens.Push(prev);
            return true;
        }

        protected override bool TryRemoveActiveToggleable(IToggleable screen)
        {
            if (base.TryRemoveActiveToggleable(screen) && _screens.Count != 0)
                base.AddActiveToggleable(_screens.Pop());
            return true;
        }
    }
}
