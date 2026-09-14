using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputDeviceSO.ActionImagePair;

[RequireComponent(typeof(Image))]
public class HintInputButton : MonoBehaviour
{
    [SerializeField] InputActionReference _inputAction;
    [Tooltip("Fill in only if the action contains composite bindings")]
    [SerializeField] string _compositePartName;
    [SerializeField] AvailableInputDevicesSO _inputDevices;

    [Header("Listens to")]
    [SerializeField] StringGameEvent _controlsChanged;

    bool IsAvailable => _frames != null;

    Image _graphic;
    StateFrames _frames;

    private void Awake()
    {
        _graphic = GetComponent<Image>();
    }

    void OnPress(InputAction.CallbackContext _) { _graphic.sprite = _frames?.Pressed; }
    void OnRelease(InputAction.CallbackContext _) { _graphic.sprite = _frames?.Released; }

    private void OnEnable()
    {
        _inputAction.action.started += OnPress;
        _inputAction.action.canceled += OnRelease;
        _controlsChanged.AddListener(SetHintSprite);

        UpdateGraphics();
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
                Debug.Log($"Couldnt find an input image for {scheme} for {_inputAction.name}.{controlPath}");
        }

        UpdateGraphics();
    }
}
