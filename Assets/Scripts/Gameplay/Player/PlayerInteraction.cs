using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerInteraction : MonoBehaviour
    {
        readonly List<IInteractable> _interactables = new(16);

        void OnInteract(InputValue _)
        {
            if (_interactables.Count != 0)
                _interactables[0].Interact();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractable>(out var interactable))
                _interactables.Add(interactable);

            if (collision.TryGetComponent<IApproachable>(out var approachable))
                approachable.Approaching();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractable>(out var interactable))
                _interactables.Remove(interactable);

            if (collision.TryGetComponent<IApproachable>(out var approachable))
                approachable.Retreating();
        }
    }
}
