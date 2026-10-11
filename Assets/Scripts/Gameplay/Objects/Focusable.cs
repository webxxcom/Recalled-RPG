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
    }
}
