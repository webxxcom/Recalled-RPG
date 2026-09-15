using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public abstract class UIScreen : ToggleableState
{
    Selectable[] _selectables;
    Canvas _canvas;

    public event Action<UIScreen, bool> OnStateChange;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _selectables = GetComponentsInChildren<Selectable>(true);
    }

    protected override void Start()
    {
        base.Start();
    }

    void ToggleNavigation(bool isNavigable)
    {
        foreach (var selectable in _selectables)
        {
            selectable.navigation = new Navigation()
            {
                mode = isNavigable ? Navigation.Mode.Automatic : Navigation.Mode.None
            };
        }
    }

    void ToggleElements(bool val)
    {
        _canvas.enabled = val;
        ToggleNavigation(val);
    }

    protected override void Activate()
    {
        ToggleElements(true);
    }

    protected override void Deactivate()
    {
        ToggleElements(false);
    }
}
