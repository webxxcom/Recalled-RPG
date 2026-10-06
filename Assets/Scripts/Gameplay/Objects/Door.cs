using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour // TODO Interactable
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

                Interacted?.Invoke();
                _isOpen = value;
                _collider2D.enabled = !value;
            }
        }

        public bool CanBeInteracted => true;

        public event Action Interacted;

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
}
