using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] bool _allowEmpty;

    List<Toggleable> _toggleables;
    protected List<Toggleable> _active = new();

    public IReadOnlyList<Toggleable> Elements
    {
        get
        {
            if (_toggleables == null)
            {
                _toggleables = new();
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent(out Toggleable screen))
                        _toggleables.Add(screen);
                }
            }
            return _toggleables;
        }
    }

    public event Action<Toggleable> ScreenChanged;

    private void OnEnable()
    {
        foreach (var screen in Elements)
            screen.ToggleRequested += ToggleToggleable;
    }

    private void OnDisable()
    {
        foreach (var screen in Elements)
            screen.ToggleRequested -= ToggleToggleable;
    }

    void ToggleToggleable(Toggleable toggleable)
    {
        if (_active.Contains(toggleable))
            RemoveActiveToggleable(toggleable);
        else AddActiveToggleable(toggleable);
    }

    protected virtual bool AddActiveToggleable(Toggleable toggleable)
    {
        // Don't want to set same screen again or more than one if disallowed
        if (toggleable == null || toggleable.IsActive)
            return false;

        if (!_allowMultiple && _active.Count > 0)
            UncheckedRemoveActiveScreen(_active[0]);

        _active.Add(toggleable);
        ((IToggleable)toggleable).SetActive(true);
        ScreenChanged?.Invoke(toggleable);
        return true;
    }

    protected virtual bool RemoveActiveToggleable(Toggleable screen)
    {
        if (screen == null || !screen.IsActive || (_active.Count == 1 && !_allowEmpty))
            return false;

        return UncheckedRemoveActiveScreen(screen);
    }

    bool UncheckedRemoveActiveScreen(Toggleable screen)
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
        AddActiveToggleable(nextTab);
    }

    public bool Request(Toggleable toggleable)
    {
        if (toggleable == null || !_toggleables.Contains(toggleable))
            return false;

        AddActiveToggleable(toggleable);
        return true;
    }
}
