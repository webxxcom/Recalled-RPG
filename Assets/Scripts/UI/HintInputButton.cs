using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static InputDeviceSO.ActionImagePair;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class HintInputButton : MonoBehaviour
{
    [SerializeField] InputActionReference _inputAction;
    [SerializeField] AvailableInputDevicesSO _inputDevices;

    [Header("Listens to")]
    [SerializeField] StringGameEvent _controlsChanged;

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
    }

    private void OnDisable()
    {
        _inputAction.action.started -= OnPress;
        _inputAction.action.canceled -= OnRelease;
        _controlsChanged.RemoveListener(SetHintSprite);
    }

    InputBinding GetBinding(InputAction inputAction, string scheme)
    {
        return scheme switch
        {
            "Keyboard&Mouse" => inputAction.bindings[0],
            "DualSense" => inputAction.bindings[1],
            _ => throw new MissingReferenceException($"Can't find a controlPath for {scheme}"),
        };
    }

    string GetControlPath(string path)
    {
        InputControlPath.ToHumanReadableString(
            path,
            out var _,
            out var controlPath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        return controlPath;
    }

    void SetHintSprite(string scheme)
    {
        string controlPath = GetControlPath(GetBinding(_inputAction.action, scheme).path);

        _frames = _inputDevices.CurrentDevice.Pairs
            .FirstOrDefault(p => p.Path.Equals(controlPath))?.Frames;
        if (_frames != null)
        {
            _graphic.sprite = _frames.Released;
        }
        else
        {
            _graphic.sprite = null;
            Debug.Log($"Couldnt find an input image for {scheme} for {_inputAction.name}.{controlPath}");
        }
    }
}
