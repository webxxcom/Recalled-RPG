using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionProcessor : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos;

        readonly HashSet<Collider2D> _overlaps = new();

        void OnInteract(InputValue _)
        {
            foreach (var collider in _overlaps)
            {
                if (collider.TryGetComponent<Interactable>(out var interactable))
                {
                    interactable.Interact(this);
                    _overlaps.Remove(collider);
                    break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _overlaps.Add(collision);

            if (collision.TryGetComponent<Approachable>(out var approachable))
                approachable.Revealed();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _overlaps.Remove(collision);

            if (collision.TryGetComponent<Approachable>(out var approachable))
                approachable.Hidden();
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            foreach (var overlap in _overlaps)
                Gizmos.DrawLine(transform.position, overlap.transform.position);
        }
    }
}
