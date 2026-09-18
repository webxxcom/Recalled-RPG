using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputWatcher : MonoBehaviour
{
    [SerializeField] StringRuntimeVariable _currentControlSchemeVariable;

    PlayerInput _playerInput;
    bool _isDirty;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnControlsChanged()
    {
        if (_playerInput == null) _isDirty = true;
        else _currentControlSchemeVariable.Value = _playerInput.currentControlScheme;
    }

    private void Update()
    {
        if (_isDirty)
        {
            _currentControlSchemeVariable.Value = _playerInput.currentControlScheme;
            _isDirty = false;
        }
    }
}
