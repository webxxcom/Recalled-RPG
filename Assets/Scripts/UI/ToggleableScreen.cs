using System;
using UnityEngine;

public abstract class ToggleableScreen : MonoBehaviour
{
    bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (value == IsOpen)
                return;

            _isOpen = value;
            UpdateState();
        }
    }

    public abstract event Action<ToggleableScreen> Toggled;

    protected virtual void Start()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (IsOpen) Show();
        else Hide();
    }

    protected virtual void Show() { }
    protected virtual void Hide() { }
    protected void Toggle() => IsOpen = !IsOpen;
}
