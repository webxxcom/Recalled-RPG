using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(PlayerInput))]
    sealed class ControlsChangedWatcher : MonoBehaviour
    {
        [SerializeField] AvailableInputDevicesSO _devices;

        [Header("Sets")]
        [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;

        void OnControlsChanged(PlayerInput playerInput)
        {
            _currentInputDevice.Value = _devices.FindForScheme(playerInput.currentControlScheme);
        }
    }
}
