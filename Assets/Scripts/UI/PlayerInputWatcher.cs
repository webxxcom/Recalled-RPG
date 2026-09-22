using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputWatcher : MonoBehaviour
{
    [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;
    [SerializeField] AvailableInputDevicesSO _devices;

    void OnControlsChanged(PlayerInput playerInput)
    {
        _currentInputDevice.Value = _devices.FindForScheme(playerInput.currentControlScheme);
    }
}
