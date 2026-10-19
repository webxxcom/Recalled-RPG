using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(EyeSight))]
    public class InteractionProcessor : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos;

        EyeSight _eyeSight;

        private void Awake()
        {
            _eyeSight = GetComponent<EyeSight>();
        }

        void OnInteract(InputValue _)
        {
            if (_currentFocused != null)
                _currentFocused.GetComponent<Interactable>().Interact(this);
        }

        Focusable _currentFocused;
        void Update()
        {
            var _currentClosest = _eyeSight.FindClosest();

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
                    if (_currentFocused.gameObject == _currentClosest)
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

            if (_currentFocused)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _currentFocused.transform.position);
            }
        }
    }
}
