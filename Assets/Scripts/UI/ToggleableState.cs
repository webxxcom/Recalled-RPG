using System;
using UnityEngine;

public abstract class ToggleableState : MonoBehaviour
{
    bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (value == IsActive)
                return;

            _isActive = value;
            UpdateState();
        }
    }

    public abstract event Action<ToggleableState> Toggled;

    protected virtual void Start()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (IsActive) Activate();
        else Deactivate();
    }

    protected virtual void Activate() { }
    protected virtual void Deactivate() { }
    protected void Toggle() => IsActive = !IsActive;
}
