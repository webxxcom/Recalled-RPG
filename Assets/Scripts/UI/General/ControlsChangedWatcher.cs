using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI.Assets.Scripts.UI.General
{
    [RequireComponent(typeof(PlayerInput))]
    public sealed class ControlsChangedWatcher : MonoBehaviour
    {
        [SerializeField] AvailableInputDevicesSO _devices;

        [Header("Sets Value to")]
        [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;

        void OnControlsChanged(PlayerInput playerInput)
        {
            _currentInputDevice.Value = _devices.FindForScheme(playerInput.currentControlScheme);
        }
    }
}
