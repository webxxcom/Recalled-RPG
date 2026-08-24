using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsSettingsMenu : MonoBehaviour
{
    [SerializeField] InputActionAsset _currentInput;
    [SerializeField] ControlBindItem _inputBindUIItem;
    [SerializeField] GameObject _uiRoot;

    private void Start()
    {
        InputActionMap map = _currentInput.FindActionMap("Player");

        foreach (InputAction action in map)
        {
            ControlBindItem cbi = Instantiate(_inputBindUIItem, _uiRoot.transform);
            cbi.Init(action);
        }
    }
}
