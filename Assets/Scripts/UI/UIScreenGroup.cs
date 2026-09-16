using System.Collections.Generic;
using UnityEngine;

public sealed class UIScreenGroup : ToggleableGroup
{
    [SerializeField] ScreenViewUi _firstView;
    readonly Stack<ToggleableObject> _screens = new();

    private void Start()
    {
        AddActiveToggleable(_firstView);
    }

    protected override bool AddActiveToggleable(ToggleableObject screen)
    {
        ToggleableObject prev = null;
        if (_active.Count != 0)
            prev = _active[0];

        if (base.AddActiveToggleable(screen) && prev != null)
            _screens.Push(prev);
        return true;
    }

    protected override bool RemoveActiveToggleable(ToggleableObject screen)
    {
        if (base.RemoveActiveToggleable(screen) && _screens.Count != 0)
            AddActiveToggleable(_screens.Pop());
        return true;
    }
}
