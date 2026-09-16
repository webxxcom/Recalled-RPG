using System;
using UnityEngine;

public class ToggleableObject : MonoBehaviour, IToggleable
{
    /// <summary>
    /// IsActive is never called withing the class or the inheritors to avoid races for being active.
    /// The manager, or handler, or anything else manages the activation of the gameobject,
    ///     not the gameobject itself.
    /// </summary>
    public bool IsActive { get; private set; }

    ///<summary>
    /// Event describing the desire to be toggled not an actual toggling.
    /// The manager then allows or disallows the call
    ///</summary>
    public event Action<ToggleableObject> ToggleRequested;

    protected void RequestToggle() => ToggleRequested?.Invoke(this);

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

    void IToggleable.SetActive(bool value)
    {
        if (value == IsActive)
            return;

        IsActive = value;
        UpdateState();
    }
}
