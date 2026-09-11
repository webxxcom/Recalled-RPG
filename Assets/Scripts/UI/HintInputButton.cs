using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputDeviceSO.ActionImagePair;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class HintInputButton : MonoBehaviour
{
    [SerializeField] InputActionReference _inputAction;
    [SerializeField] InputDeviceSO _inputDevice;

    Image _graphic;
    PlayerInput _playerInput;
    StateFrames _frames;

    InputDeviceSO GetCurrentDeviceMap()
    {
        foreach (var device in _playerInput.devices)
            if (device.displayName.Equals(_inputDevice.name))
                return _inputDevice;
        return null;
    }

    void FindBindingFrames()
    {
        foreach (var bind in _inputDevice.Binds)
        {
            if (_inputDevice.HaveSamePath(bind, _inputAction.action.bindings[0]))
            {
                _frames = bind.Frames;
                break;
            }
        }
    }

    private void Awake()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
    }

    void OnPress(InputAction.CallbackContext _) { _graphic.sprite = _frames.Pressed; }
    void OnRelease(InputAction.CallbackContext _) { _graphic.sprite = _frames.Released; }

    private void OnEnable()
    {
        _inputAction.action.started += OnPress;
        _inputAction.action.canceled += OnRelease;
    }

    private void OnDisable()
    {
        _inputAction.action.started -= OnPress;
        _inputAction.action.canceled -= OnRelease;
    }

#if UNITY_EDITOR

    InputActionReference _prevAction;
    private void OnValidate()
    {
        if (_graphic == null) _graphic = GetComponent<Image>();
        if (_inputDevice == null)
        {
            _graphic.sprite = null;
            _frames = null;
        }

        if (_inputDevice != null && _prevAction != _inputAction)
        {
            FindBindingFrames();
            _graphic.sprite = _frames.Released;
            _prevAction = _inputAction;
        }
    }
#endif
}
