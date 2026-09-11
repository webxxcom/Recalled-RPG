using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public abstract class UIScreen : MonoBehaviour
{
    Selectable[] _selectables;
    Canvas _canvas;

    public bool IsActive
    {
        get => _canvas.enabled;
        set
        {
            if (value == IsActive)
                return;

            SetIsActiveWithoutNotify(value);
            OnStateChange?.Invoke(this, value);
        }
    }

    public event Action<UIScreen, bool> OnStateChange;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        _selectables = GetComponentsInChildren<Selectable>();
        SetIsActiveWithoutNotify(false);
    }

    protected void Toggle() => IsActive = !IsActive;

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

    void SetIsActiveWithoutNotify(bool val)
    {
        _canvas.enabled = val;
        ToggleNavigation(val);

        if (val) Open();
        else Close();
    }

    public virtual void Open() { }
    public virtual void Close() { }
}
