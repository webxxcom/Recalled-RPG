using System.Collections.Generic;
using UnityEngine;

public sealed class UIScreenGroup : ToggleableGroup
{
    [SerializeField] GameStateSO _baseGameState;
    readonly Stack<Toggleable> _screens = new();

    protected override bool AddActiveToggleable(Toggleable screen)
    {
        Toggleable prev = _active.Count != 0 ? _active[0] : null;

        if (base.AddActiveToggleable(screen) && prev != null)
            _screens.Push(prev);
        return true;
    }

    protected override bool RemoveActiveToggleable(Toggleable screen)
    {
        if (base.RemoveActiveToggleable(screen) && _screens.Count != 0)
            AddActiveToggleable(_screens.Pop());
        return true;
    }
}
