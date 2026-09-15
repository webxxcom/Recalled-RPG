using System;
using UnityEngine;

public abstract class ToggleableObject : MonoBehaviour
{
    /// <summary>
    /// IsActive is never called withing the class or the inheritors to avoid races for being active.
    /// The manager, or handler, or anything else manages the activation of the gameobject,
    ///     not the gameobject itself.
    /// </summary>
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

    ///<summary>
    ///Event describing the desire to be toggled not an actual toggling.
    ///The manager then allows or disallows the call
    ///</summary>
    public abstract event Action<ToggleableObject> Toggled;

    protected virtual void Start()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (IsActive) Activate();
        else Deactivate();
    }

    /// <summary>
    /// Virtual methods representing instructions to complete after the desire to be toggled is fulfilled
    /// </summary>
    protected virtual void Activate() { }
    protected virtual void Deactivate() { }
    protected void Toggle() => IsActive = !IsActive;
}
