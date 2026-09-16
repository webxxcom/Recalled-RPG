using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// Component used to group a set of gameobject which have the <strong>ToggleableScreen</strong> component.
/// The discovering happens only at the immediate children to avoid grouping the gameobjects from down the hierarchy.
/// 
/// The main purpose of this component is to provide an activation of screens within a group using an event
///     <strong>ToggleableScreen.Toggled</strong>. The component itself only manages whether
///     the screen should be turned on or not which wholly depends whether the screen
///     is active or multiple screens are allowed
/// 
/// </summary>
public class ToggleableGroup : MonoBehaviour
{
    [SerializeField] bool _allowMultiple;

    List<ToggleableObject> _toggleables;
    [SerializeField] protected List<ToggleableObject> _active = new();

    public IReadOnlyList<ToggleableObject> Elements => _toggleables;

    public event Action<ToggleableObject> ScreenChanged;

    private void Awake()
    {
        GetImmeaditeChildren();
    }

    private void OnEnable()
    {
        foreach (var screen in _toggleables)
            screen.ToggleRequested += ToggleSreen;
    }

    private void OnDisable()
    {
        foreach (var screen in _toggleables)
            screen.ToggleRequested -= ToggleSreen;
    }

    void ToggleSreen(ToggleableObject screen)
    {
        if (_active.Contains(screen))
            RemoveActiveScreen(screen);
        else AddActiveScreen(screen);
    }

    protected virtual bool AddActiveScreen(ToggleableObject screen)
    {
        // Don't want to set same screen again or more than one if disallowed
        if (screen == null || screen.IsActive)
            return false;

        if (!_allowMultiple && _active.Count > 0)
            UncheckedRemoveActiveScreen(_active[0]);

        _active.Add(screen);
        ((IToggleable)screen).SetActive(true);
        ScreenChanged?.Invoke(screen);
        return true;
    }

    protected virtual bool RemoveActiveScreen(ToggleableObject screen)
    {
        if (screen == null || !screen.IsActive)
            return false;

        return UncheckedRemoveActiveScreen(screen);
    }

    bool UncheckedRemoveActiveScreen(ToggleableObject screen)
    {
        if (!_active.Remove(screen))
            return false;

        ((IToggleable)screen).SetActive(false);
        return true;
    }

    public void ChooseNext(int offset)
    {
        if (_allowMultiple)
            return;

        if (offset < 0)
            offset = _toggleables.Count + offset;

        var nextTab = _toggleables[(_toggleables.IndexOf(_active[0]) + offset) % _toggleables.Count];
        AddActiveScreen(nextTab);
    }

    public bool RequestScreen(ToggleableObject toggleable)
    {
        if (toggleable == null || !_toggleables.Contains(toggleable))
            return false;

        AddActiveScreen(toggleable);
        return true;
    }

    void GetImmeaditeChildren()
    {
        _toggleables = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ToggleableObject screen))
                _toggleables.Add(screen);
        }
    }
}
