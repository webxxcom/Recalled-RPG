using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Recalled.UI.InputDeviceSO.ActionImagePair;

namespace Recalled.UI
{
    /// <summary>
    /// The component describes an object which shows the current binding by a connected device to the serialized action
    /// 
    /// The visuals are changed only in 2 cases:
    ///     1. The button is currently observable and controls are changed;
    ///     2. The button was activated
    /// 
    /// The second point makes it difficult because the button should make a request for current binding or smth should tell it.
    /// The object activating the button should populate current control scheme
    /// </summary>
    [RequireComponent(typeof(Image))]
    public sealed class HintInputButton : MonoBehaviour
    {
        [SerializeField] Image _graphic;
        [SerializeField] InputActionReference _inputAction;
        [Tooltip("Fill in only if the action contains composite bindings")]
        [SerializeField] string _compositePartName;
        [SerializeField] bool _listenToInput;
        [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;
        [SerializeField] bool _showPressed;

        bool IsAvailable => _frames != null;
        StateFrames _frames;

        void UpdateGraphics(Sprite sprite)
        {
            _graphic.sprite = sprite;
            _graphic.preserveAspect = true;
        }

        void OnPress(InputAction.CallbackContext _)
        {
            if (_listenToInput) UpdateGraphics(_frames?.Pressed);
        }
        void OnRelease(InputAction.CallbackContext _)
        {
            if (_listenToInput) UpdateGraphics(_frames?.Released);
        }

        private void OnEnable()
        {
            _inputAction.action.started += OnPress;
            _inputAction.action.canceled += OnRelease;
            _currentInputDevice.Subscribe(OnInputDeviceChanged);

            if (_showPressed) UpdateGraphics(_frames?.Pressed);
        }

        private void OnDisable()
        {
            _inputAction.action.started -= OnPress;
            _inputAction.action.canceled -= OnRelease;
            _currentInputDevice.Unsubscribe(OnInputDeviceChanged);
        }

        void UpdateGraphics()
        {
            if (IsAvailable)
            {
                _graphic.enabled = true;
                _graphic.sprite = _frames.Released;
            }
            else
            {
                _graphic.enabled = false;
            }
        }

        void OnInputDeviceChanged(InputDeviceSO inputDevice)
        {
            if (inputDevice == null)
                return;

            _frames = inputDevice.GetFramesForAction(_inputAction.action, _compositePartName);
            UpdateGraphics();
        }

        InputDeviceSO _prev;
        private void OnValidate()
        {
            if (_graphic == null)
                _graphic = GetComponent<Image>();

            if (_currentInputDevice.InitialValue != null && _prev != _currentInputDevice.Value && _inputAction != null)
            {
                OnInputDeviceChanged(_currentInputDevice.InitialValue);
                _prev = _currentInputDevice.InitialValue;
            }
        }
    }
}
