using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateStackHandler : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] GameState _baseState;
    [SerializeField] SettingsConfig _settings;

    List<StateHandler> _allScreens;

    readonly Stack<GameState> _states = new();

    void Awake()
    {
        _allScreens = GetComponents<StateHandler>().ToList();

        _states.Push(_baseState);
        ApplyCurrentState();
    }

    void Start()
    {
        _settings.Load();
    }

    private void OnEnable()
    {
        foreach (var state in _allScreens)
            state.Toggled += OnToggle;
    }

    private void OnDisable()
    {
        foreach (var state in _allScreens)
            state.Toggled -= OnToggle;
    }

    void OnToggle(ToggleableScreen screen)
    {
        if (screen is StateHandler stateHandler)
        {
            if (_states.Peek() != screen)
                TryAdd(stateHandler);
            else
                TryRemove(stateHandler);
        }
    }

    void ApplyCurrentState()
    {
        GameState current = _states.Peek();

        _playerInput.actions.Disable();
        foreach (var am in current.ActionMaps)
            _playerInput.actions.FindActionMap(am, throwIfNotFound: true).Enable();
        Time.timeScale = current.FreezeTime ? 0f : 1f;
        Cursor.lockState = current.CursorMode;
    }

    bool TryAdd(StateHandler state)
    {
        if (_states.Count != 0 && (_states.Peek().BlockedStates?.Contains(state.Definition) == null))
            return false;

        _states.Push(state.Definition);
        state.IsOpen = true;

        ApplyCurrentState();
        return true;
    }

    bool TryRemove(StateHandler state)
    {
        if (_states.Count == 1)
        {
            Debug.LogError($"Cannot pop last element from {nameof(StateStackHandler)}.");
            return false;
        }
        if (_states.Peek() != state)
            return false;

        _states.Pop();
        state.IsOpen = false;

        ApplyCurrentState();
        return true;
    }
}
