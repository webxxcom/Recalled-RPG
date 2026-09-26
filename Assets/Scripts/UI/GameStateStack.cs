using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Owns the game state stack for one scene and derives all state-dependent
/// settings (input maps, time scale, cursor) from the top frame.
///
/// Deliberately knows nothing about UI, dialogue, or pausing. Callers push a
/// state, keep the returned handle, and release it when they're done. Adding a
/// new state requires no change to this class.
///
/// Views (e.g. a UI presenter) listen to <see cref="Changed"/> and query the
/// stack; the stack never reaches out to them.
/// </summary>
public sealed class GameStateStack : MonoBehaviour
{
    [Tooltip("Bottom frame for this scene. Always present, cannot be released.")]
    [SerializeField] GameStateSO _baseState;

    [Tooltip("Wired explicitly. Never looked up at runtime.")]
    [SerializeField] PlayerInput _playerInput;

    [Tooltip("Maps enabled in every state, e.g. System (pause/back). Owned here so no state can forget them.")]
    [SerializeField] string[] _alwaysEnabledMaps = { "System" };

    readonly List<GameStateHandle> _frames = new();

    bool _dirty;

    /// <summary>
    /// Raised after a change has been applied (once per frame at most).
    /// Not raised for the initial base state: subscribers should read the
    /// current stack when they start, then react to changes.
    /// </summary>
    public event Action Changed;

    public GameStateSO CurrentState => _frames[^1].State;

    void Awake()
    {
        if (_baseState == null)
        {
            Debug.LogError($"{nameof(GameStateStack)}: base state not assigned.", this);
            enabled = false;
            return;
        }

        if (_playerInput == null)
        {
            Debug.LogError($"{nameof(GameStateStack)}: PlayerInput not assigned.", this);
            enabled = false;
            return;
        }

        _frames.Add(new(_baseState));
        Apply();
    }

    /// <summary>
    /// Push a state. Keep the returned handle; you need it to release.
    /// Returns null if the push was refused (the current top disallows it).
    /// </summary>
    public GameStateHandle Push(GameStateSO state)
    {
        if (state == null)
        {
            Debug.LogError($"{nameof(GameStateStack)}: pushed a null state.", this);
            return null;
        }

        if (_frames.Count == 0)
            return null; // misconfigured; already reported in Awake

        // Checked against the logical top, which is correct even if the
        // previous push hasn't been applied yet this frame.
        if (CurrentState.Disallows(state))
            return null;

        var handle = new GameStateHandle(state);
        _frames.Add(handle);
        _dirty = true;
        return handle;
    }

    /// <summary>
    /// Release a previously pushed frame, wherever it is in the stack.
    /// Safe to call with null or an already-released handle.
    /// </summary>
    public void Release(GameStateHandle handle)
    {
        if (handle == null)
            return;

        int index = _frames.IndexOf(handle);
        if (index < 0)
            return;

        if (index == 0)
        {
            Debug.LogError($"{nameof(GameStateStack)}: attempted to release the base frame.", this);
            return;
        }

        // Releasing from the middle is legal here (e.g. dialogue ending under pause).
        _frames.RemoveAt(index);
        _dirty = true;
    }

    /// <summary>
    /// Position of the top-most frame holding <paramref name="state"/>, or -1.
    /// Higher means closer to the top; useful for deriving draw order.
    /// </summary>
    public int IndexOf(GameStateSO state)
    {
        for (int i = _frames.Count - 1; i >= 0; i--)
        {
            if (_frames[i].State == state)
                return i;
        }
        return -1;
    }

    public bool Contains(GameStateSO state) => IndexOf(state) >= 0;

    /// <summary>
    /// Deferred apply. Push/Release only mutate the list and mark it dirty;
    /// the actual switch happens once, here.
    ///
    /// Disabling an action map cancels its in-flight actions, and their
    /// callbacks fire synchronously. If one of them pushes or releases, we'd
    /// otherwise re-enter mid-apply. The flag is cleared before applying, so
    /// any change made during Apply or Changed is simply picked up next frame.
    ///
    /// LateUpdate still runs when Time.timeScale is 0, so unpausing works.
    /// </summary>
    void LateUpdate()
    {
        if (!_dirty)
            return;

        _dirty = false;
        Apply();
        Changed?.Invoke();
    }

    /// <summary>
    /// Derives every state-dependent setting from the top frame.
    /// Maps are only toggled when their enabled status actually changes: a map
    /// that stays enabled across the transition is never touched, so held
    /// inputs on it (and on the System map) are not cancelled.
    /// </summary>
    void Apply()
    {
        GameStateSO state = CurrentState;
        InputActionAsset actions = _playerInput.actions;

        ReportUnknownMaps(actions, state);

        foreach (InputActionMap map in actions.actionMaps)
        {
            bool shouldEnable = NameIn(_alwaysEnabledMaps, map.name) || NameIn(state.ActionMaps, map.name);

            if (shouldEnable && !map.enabled)
                map.Enable();
            else if (!shouldEnable && map.enabled)
                map.Disable();
        }

        Time.timeScale = state.FreezeTime ? 0f : 1f;
        Cursor.lockState = state.CursorMode;
    }

    /// <summary>Map names are strings, so a typo is caught here, loudly, instead of silently doing nothing.</summary>
    void ReportUnknownMaps(InputActionAsset actions, GameStateSO state)
    {
        foreach (string name in state.ActionMaps)
        {
            if (actions.FindActionMap(name) == null)
                Debug.LogError($"{nameof(GameStateStack)}: state '{state.name}' lists unknown action map '{name}'.", state);
        }
    }

    static bool NameIn(IReadOnlyList<string> names, string value)
    {
        for (int i = 0; i < names.Count; i++)
        {
            if (string.Equals(names[i], value, StringComparison.Ordinal))
                return true;
        }
        return false;
    }
}