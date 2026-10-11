using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionProcessor : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos;

        readonly List<Collider2D> _overlaps = new();

        void OnInteract(InputValue _)
        {
            if (_currentFocused != null)
                _currentFocused.GetComponent<Interactable>().Interact(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _overlaps.Add(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _overlaps.Remove(collision);
        }

        Collider2D FindClosest()
        {
            Collider2D closest = default;
            float minDist = float.MaxValue;

            foreach (var collider in _overlaps)
            {
                var dist = Vector2.Distance(transform.position, collider.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = collider;
                }
            }
            return closest;
        }

        Focusable _currentFocused;
        void Update()
        {
            var _currentClosest = FindClosest();

            if (_currentClosest == null)
            {
                if (_currentFocused == null)
                    return;

                _currentFocused.Focused = false;
                _currentFocused = null;
            }
            else
            {
                if (_currentFocused != null)
                {
                    if (_currentFocused.gameObject == _currentClosest.gameObject)
                        return;

                    _currentFocused.Focused = false;
                }

                _currentFocused = _currentClosest.GetComponent<Focusable>();
                _currentFocused.Focused = true;
            }
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            foreach (var overlap in _overlaps)
                Gizmos.DrawLine(transform.position, overlap.transform.position);

            if (_currentFocused)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _currentFocused.transform.position);
            }
        }
    }
}
