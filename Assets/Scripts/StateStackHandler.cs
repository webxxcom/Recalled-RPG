using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateStackHandler : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] GameState _baseState;
    [SerializeField] SettingsConfig _settings;

    readonly Stack<GameState> _states = new();

    void Awake()
    {
        UncheckedAdd(_baseState);
    }

    void Start()
    {
        _settings.Load();
    }

    void UpdateState()
    {
        GameState current = _states.Peek();

        _playerInput.actions.Disable();
        foreach (var am in current.ActionMaps)
            _playerInput.actions.FindActionMap(am, throwIfNotFound: true).Enable();
        Time.timeScale = current.FreezeTime ? 0f : 1f;
        Cursor.lockState = current.CursorMode;
    }

    void UncheckedAdd(GameState state)
    {
        _states.Push(state);

        UpdateState();
    }

    public bool Add(GameState state)
    {
        if (_states.Peek().BlockedStates?.Contains(state) ?? true)
            return false;

        UncheckedAdd(state);
        return true;
    }

    public bool Remove(GameState state)
    {
        if (_states.Count == 1)
        {
            Debug.LogError($"Cannot pop last element from {nameof(StateStackHandler)}.");
            return false;
        }
        if (_states.Peek() != state)
            return false;

        _states.Pop();
        UpdateState();
        return true;
    }
}
