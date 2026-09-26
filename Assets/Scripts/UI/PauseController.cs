using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
public sealed class PauseController : MonoBehaviour
{
    [SerializeField] GameStateStack _stack;
    [SerializeField] GameStateSO _pauseState;
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] InputActionReference _pauseAction;

    GameStateHandle _handle;
    Canvas _canvas;

    public bool IsPaused => _handle != null;

    void Awake()
    {
        if (_stack == null || _pauseState == null || _playerInput == null || _pauseAction == null || _pauseAction.action == null)
        {
            Debug.LogError($"{nameof(PauseController)}: missing references.", this);
            enabled = false;
            return;
        }

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = true;
        _pauseAction.action.canceled += OnPausePerformed;
    }

    private void OnDestroy()
    {
        _pauseAction.action.canceled -= OnPausePerformed;
    }

    void OnPausePerformed(InputAction.CallbackContext _) => TogglePause();

    public void TogglePause()
    {
        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (IsPaused)
            return;

        // Null if refused (the current top state disallows Pause), which
        // correctly leaves us "not paused". The screen needs no special case:
        // nothing was pushed, so nothing is shown.
        _handle = _stack.Push(_pauseState);
    }

    public void Resume()
    {
        if (!IsPaused)
            return;

        // Guard for scene teardown, where the stack may already be destroyed.
        if (_stack != null)
            _stack.Release(_handle);

        _handle = null;
    }
}