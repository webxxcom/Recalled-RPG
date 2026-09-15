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
/// 
/// </summary>
public class ScreenGroup : MonoBehaviour
{
    [SerializeField] bool _allowMultiple;
    [SerializeField] bool _allowEmpty;
    [SerializeField] ToggleableScreen _defaultScreen;

    List<ToggleableScreen> _screens;
    [SerializeField] List<ToggleableScreen> _activeScreens = new();

    private void Awake()
    {
        _screens = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ToggleableScreen screen))
                _screens.Add(screen);
        }

        if (!_allowEmpty && _defaultScreen == null)
            Debug.LogException(new MissingReferenceException("Can't have a non-empty group with no default screen"), this);
    }

    private void OnEnable()
    {
        foreach (var screen in _screens)
            screen.Toggled += ToggleSreen;
    }

    private void OnDisable()
    {
        foreach (var screen in _screens)
            screen.Toggled -= ToggleSreen;
    }

    private void Start()
    {
        AddActiveScreen(_defaultScreen);
    }

    void ToggleSreen(ToggleableScreen screen)
    {
        if (_allowEmpty && screen.IsOpen && !_activeScreens.Contains(screen))
            RemoveActiveScreen(screen);

        AddActiveScreen(screen);
    }


    void AddActiveScreen(ToggleableScreen screen)
    {
        // Don't want to set same screen again or more than one if disallowed
        if (screen == null || screen.IsOpen)
            return;

        if (!_allowMultiple && _activeScreens.Count > 0)
            UncheckedRemoveActiveScreen(_activeScreens[0]);

        _activeScreens.Add(screen);
        screen.IsOpen = true;
    }

    void RemoveActiveScreen(ToggleableScreen screen)
    {
        if (screen == null || !screen.IsOpen || (_activeScreens.Count == 1 && !_allowEmpty))
            return;

        UncheckedRemoveActiveScreen(screen);
    }

    void UncheckedRemoveActiveScreen(ToggleableScreen screen)
    {
        if (_activeScreens.Remove(screen))
            screen.IsOpen = false;
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
}
