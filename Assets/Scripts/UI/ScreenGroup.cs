using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenGroup : MonoBehaviour
{
    List<ToggleableScreen> _screens;
    ToggleableScreen _activeScreen;

    private void Awake()
    {
        _screens = new();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ToggleableScreen screen))
                _screens.Add(screen);
        }
    }

    private void OnEnable()
    {
        foreach (var screen in _screens)
            screen.Toggled += SetActiveScreen;
    }

    private void OnDisable()
    {
        foreach (var screen in _screens)
            screen.Toggled -= SetActiveScreen;
    }

    private void Start()
    {
        SetActiveScreen(_screens.First());
    }

    void SetActiveScreen(ToggleableScreen tab)
    {
        // Don't want to set same screen again
        if (tab == null || tab == _activeScreen)
            return;

        if (_activeScreen != null)
            _activeScreen.IsOpen = false;

        _activeScreen = tab;
        _activeScreen.IsOpen = true;
    }

    public void ShiftActiveScreen(int offset)
    {
        if (offset < 0)
            offset = _screens.Count + offset;

        var nextTab = _screens[(_screens.IndexOf(_activeScreen) + offset) % _screens.Count];
        SetActiveScreen(nextTab);
    }
}
