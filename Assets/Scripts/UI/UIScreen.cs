using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIScreen : ToggleableObject
{
    [SerializeField] GameState _gameState;
    [SerializeField] PlayerInput _playerInput;

    [Tooltip("Game Event which toggles(!) the UI screen")]
    [SerializeField] VoidGameEvent OnGameEventRaised;

    Selectable[] _selectables;
    Canvas _canvas;

    public override event Action<ToggleableObject> Toggled;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _selectables = GetComponentsInChildren<Selectable>(true);
    }

    void Raise() => Toggled?.Invoke(this);
    void OnEnable()
        => OnGameEventRaised.OnEventRaised += Raise;
    void OnDisable()
        => OnGameEventRaised.OnEventRaised -= Raise;

    void ToggleNavigation(bool isNavigable)
    {
        foreach (var selectable in _selectables)
        {
            selectable.navigation = new Navigation()
            {
                mode = isNavigable ? Navigation.Mode.Automatic : Navigation.Mode.None
            };
        }
    }

    void ToggleElements(bool val)
    {
        _canvas.enabled = val;
        ToggleNavigation(val);
    }

    protected override void Activate()
    {
        ToggleElements(true);
        ApplyCurrentState();
    }

    protected override void Deactivate()
    {
        ToggleElements(false);
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
