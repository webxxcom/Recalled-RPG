using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    public sealed class Focusable : MonoBehaviour
    {
        [SerializeField] UnityEvent<bool> _focused;

        public bool Focused
        {
            set => _focused?.Invoke(value);
        }

        private void Start()
        {
            _focused.Invoke(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<InteractionProcessor>(out var _))
                _focused.Invoke(true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<InteractionProcessor>(out var _))
                _focused.Invoke(false);
        }
    }
}
