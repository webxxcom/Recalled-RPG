using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputWatcher : MonoBehaviour
{
    [SerializeField] AvailableInputDevicesSO _devices;

    [Header("Sets Value to")]
    [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;

    void OnControlsChanged(PlayerInput playerInput)
    {
        _currentInputDevice.Value = _devices.FindForScheme(playerInput.currentControlScheme);
    }
}
