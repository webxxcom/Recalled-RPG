using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Toggleable))]
public class ToggleableGameStateApllier : MonoBehaviour
{
    [SerializeField] GameState _gameState;
    [SerializeField] PlayerInput _playerInput;

    Toggleable _toggleable;

    void Awake()
    {
        _toggleable = GetComponent<Toggleable>();

        _toggleable.Activated += ApplyCurrentState;
    }

    void OnDestroy()
    {
        _toggleable.Activated -= ApplyCurrentState;
    }

    void ApplyCurrentState()
    {
        _playerInput.actions.Disable();
        foreach (var am in _gameState.ActionMaps)
            _playerInput.actions.FindActionMap(am, throwIfNotFound: true).Enable();
        Time.timeScale = _gameState.FreezeTime ? 0f : 1f;
        Cursor.lockState = _gameState.CursorMode;
    }
}
