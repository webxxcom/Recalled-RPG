using System.Linq;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputDeviceSO.ActionImagePair;

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
public class HintInputButton : MonoBehaviour
{
    [SerializeField] InputActionReference _inputAction;
    [Tooltip("Fill in only if the action contains composite bindings")]
    [SerializeField] string _compositePartName;
    [SerializeField] AvailableInputDevicesSO _inputDevices;
    [SerializeField] bool _listenToInput;

    [Header("Listens to")]
    [SerializeField] StringGameEvent _controlsChanged;

    bool IsAvailable => _frames != null;

    Image _graphic;
    StateFrames _frames;

    private void Awake()
    {
        _graphic = GetComponent<Image>();
    }

    void UpdateGraphics(Sprite sprite)
    {
        _graphic.sprite = sprite;
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
        _controlsChanged.AddListener(SetHintSprite);

        SetHintSprite(PlayerInputWatcher.CurrentScheme);
    }

    private void OnDisable()
    {
        _inputAction.action.started -= OnPress;
        _inputAction.action.canceled -= OnRelease;
        _controlsChanged.RemoveListener(SetHintSprite);
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

    void SetHintSprite(string scheme)
    {
        string controlPath = ControlSchemeBindings.GetControlPathNoDevice(_inputAction.action, scheme, _compositePartName);

        // We may not have a keyboard representation for each action
        if (string.IsNullOrEmpty(controlPath))
            _frames = null;
        else
        {
            _frames = _inputDevices.CurrentDevice.Pairs
                .FirstOrDefault(p => p.Path.Equals(controlPath))?.Frames;

            // Debug message just in case
            if (_frames == null)
                Debug.LogError($"Couldnt find an input image for {scheme} for {_inputAction.name}.{controlPath}", this);
        }

        UpdateGraphics();
    }
}
