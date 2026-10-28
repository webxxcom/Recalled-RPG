using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(StateScreen))]
    public class ScreenOnActionBinder : MonoBehaviour
    {
        [SerializeField] InputActionReference _actionReference;

        Toggleable _toggleable;
        private void Awake()
        {
            _toggleable = GetComponent<Toggleable>();

            _actionReference.action.canceled += OnCancelled;
        }
        private void OnDestroy() => _actionReference.action.canceled -= OnCancelled;
        void OnCancelled(InputAction.CallbackContext _)
            => _toggleable.RequestToggle();
    }
}
