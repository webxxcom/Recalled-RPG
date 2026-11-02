using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    internal sealed class Interactable : MonoBehaviour
    {
        static readonly string LAYER_NAME = "Interactable";

        public Action<InteractionProcessor> RequestInteraction;

        Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            var interactableMask = LayerMask.NameToLayer(LAYER_NAME);
            if (interactableMask == -1)
                throw new ArgumentException($"No layer called {LAYER_NAME}");

            _collider.includeLayers |= (1 << interactableMask);
        }

        public void Interact(InteractionProcessor interactor)
        {
            RequestInteraction?.Invoke(interactor);
        }
    }
}
