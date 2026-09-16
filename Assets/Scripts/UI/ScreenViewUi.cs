using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenViewUi : UiView
{
    [SerializeField] GameState _gameState;
    [SerializeField] PlayerInput _playerInput;
    [Tooltip("Game Event which toggles(!) the UI screen")]
    [SerializeField] VoidGameEvent OnGameEventRaised;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnGameEventRaised.OnEventRaised += _toggleable.RequestToggle;
        _toggleable.Activated += ApplyCurrentState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnGameEventRaised.OnEventRaised -= _toggleable.RequestToggle;
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
