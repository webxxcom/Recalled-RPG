using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenViewUi : UiView
{
    [SerializeField] GameState _gameState;
    [SerializeField] PlayerInput _playerInput;
    [Tooltip("Game Event which toggles(!) the UI screen")]
    [SerializeField] VoidGameEvent OnGameEventRaised;

    public override event Action<ToggleableObject> Toggled;

    void Raise() => Toggled?.Invoke(this);
    void OnEnable()
        => OnGameEventRaised.OnEventRaised += Raise;
    void OnDisable()
        => OnGameEventRaised.OnEventRaised -= Raise;

    protected override void Activate()
    {
        base.Activate();

        ApplyCurrentState();
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
