using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Toggleable : MonoBehaviour, IToggleable
{
    [SerializeField] bool _hidesWhenInactive;

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
    public event Action<Toggleable> ToggleRequested;

    public event Action Activated;
    public event Action Deactivated;

    public void RequestToggle() => ToggleRequested?.Invoke(this);

    void Start()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (IsActive) Activate();
        else Deactivate();
    }

    void Activate() { if (_hidesWhenInactive) gameObject.SetActive(true); Activated?.Invoke(); }
    void Deactivate() { if (_hidesWhenInactive) gameObject.SetActive(false); Deactivated?.Invoke(); }

    void IToggleable.SetActive(bool value)
    {
        if (value == IsActive)
            return;

        IsActive = value;
        UpdateState();
    }
}
