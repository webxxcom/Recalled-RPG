using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
public sealed class ScreenController : MonoBehaviour
{
    [SerializeField] GameStateStack _stack;
    [SerializeField] GameStateSO _state;
    [SerializeField] InputActionReference _action;

    GameStateHandle _handle;
    Canvas _canvas;

    public bool IsActive => _handle != null;

    void Awake()
    {
        if (_stack == null || _state == null)
        {
            Debug.LogError($"{nameof(ScreenController)}: missing references.", this);
            enabled = false;
            return;
        }

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = true;

        if (_action) _action.action.canceled += OnActionPerformed;
    }

    private void OnDestroy()
    {
        if (_action) _action.action.canceled -= OnActionPerformed;
    }

    void OnActionPerformed(InputAction.CallbackContext _) => Toggle();

    public void Toggle()
    {
        if (IsActive) Deactivate();
        else Activate();
    }

    public void Activate()
    {
        if (IsActive)
            return;

        _handle = _stack.Push(_state);
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        // What if stack was destroyed
        if (_stack != null)
            _stack.Release(_handle);

        _handle = null;
    }
}