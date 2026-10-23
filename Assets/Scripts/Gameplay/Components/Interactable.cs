using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Focusable))]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Interactable : MonoBehaviour
    {
        public Action<InteractionProcessor> RequestInteraction;

        Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            var interactableMask = LayerMask.NameToLayer("Interactable");
            if (interactableMask == -1)
                throw new ArgumentException();

            _collider.includeLayers |= (1 << interactableMask);

        }

        public void Interact(InteractionProcessor interactor)
        {
            RequestInteraction?.Invoke(interactor);
        }
    }
}
