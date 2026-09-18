using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputWatcher : MonoBehaviour
{
    [SerializeField] StringRuntimeVariable _currentControlSchemeVariable;

    PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        _currentControlSchemeVariable.Value = _playerInput.currentControlScheme;
    }
}
