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
public class ScreenGroup : MonoBehaviour
{
    [SerializeField] bool _allowMultiple;
    [SerializeField] bool _allowEmpty;
    [SerializeField] ToggleableObject _defaultScreen;

    List<ToggleableObject> _screens;
    [SerializeField] protected List<ToggleableObject> _activeScreens = new();

    public event Action<ToggleableObject> ScreenChanged;

    private void Awake()
    {
        _screens = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ToggleableObject screen))
                _screens.Add(screen);
        }

        if (!_allowEmpty && _defaultScreen == null)
            Debug.LogException(new MissingReferenceException("Can't have a non-empty group with no default screen"), this);
    }

    private void OnEnable()
    {
        foreach (var screen in _screens)
            screen.ToggleRequested += ToggleSreen;
    }

    private void OnDisable()
    {
        foreach (var screen in _screens)
            screen.ToggleRequested -= ToggleSreen;
    }

    private void Start()
    {
        AddActiveScreen(_defaultScreen);
    }

    void ToggleSreen(ToggleableObject screen)
    {
        if (_allowEmpty && screen.IsActive && _activeScreens.Contains(screen))
            RemoveActiveScreen(screen);
        else AddActiveScreen(screen);
    }

    protected virtual bool AddActiveScreen(ToggleableObject screen)
    {
        // Don't want to set same screen again or more than one if disallowed
        if (screen == null || screen.IsActive)
            return false;

        if (!_allowMultiple && _activeScreens.Count > 0)
            UncheckedRemoveActiveScreen(_activeScreens[0]);

        _activeScreens.Add(screen);
        ((IToggleable)screen).SetActive(true);
        ScreenChanged?.Invoke(screen);
        return true;
    }

    protected virtual bool RemoveActiveScreen(ToggleableObject screen)
    {
        if (screen == null || !screen.IsActive || (_activeScreens.Count == 1 && !_allowEmpty))
            return false;

        return UncheckedRemoveActiveScreen(screen);
    }

    bool UncheckedRemoveActiveScreen(ToggleableObject screen)
    {
        if (!_activeScreens.Remove(screen))
            return false;

        ((IToggleable)screen).SetActive(false);
        return true;
    }

    public void ChooseNext(int offset)
    {
        if (_allowMultiple)
            return;

        if (offset < 0)
            offset = _screens.Count + offset;

        var nextTab = _screens[(_screens.IndexOf(_activeScreens[0]) + offset) % _screens.Count];
        AddActiveScreen(nextTab);
    }

    public bool RequestScreen(ToggleableObject toggleable)
    {
        if (toggleable == null || !_screens.Contains(toggleable))
            return false;

        AddActiveScreen(toggleable);
        return true;
    }
}
