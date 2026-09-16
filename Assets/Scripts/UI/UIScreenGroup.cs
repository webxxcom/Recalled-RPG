using System.Collections.Generic;

public sealed class UIScreenGroup : ToggleableGroup
{
    readonly Stack<ToggleableObject> _screens = new();

    protected override bool AddActiveScreen(ToggleableObject screen)
    {
        ToggleableObject prev = null;
        if (_active.Count != 0)
            prev = _active[0];

        if (base.AddActiveScreen(screen) && prev != null)
            _screens.Push(prev);
        return true;
    }

    protected override bool RemoveActiveScreen(ToggleableObject screen)
    {
        if (base.RemoveActiveScreen(screen) && _screens.Count != 0)
            AddActiveScreen(_screens.Pop());
        return true;
    }
}
