using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour, IInteractable
{
    Collider2D _collider2D;
    bool _isOpen;
    bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (_isOpen == value)
                return;

            OnInteract?.Invoke();
            _isOpen = value;
            _collider2D.enabled = !value;
        }
    }

    public event Action OnInteract;

    void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _isOpen = !_collider2D.enabled;
    }

    public void Open()
        => IsOpen = true;
    public void Close()
        => IsOpen = false;
    public void Interact()
        => IsOpen = !IsOpen;
}
