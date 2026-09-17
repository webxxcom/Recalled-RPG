using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputWatcher : MonoBehaviour
{
    [Header("Raises")]
    [SerializeField] StringGameEvent _controlSchemeChanged;

    public static string CurrentScheme { get; private set; }

    bool _isDirty;
    PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (_isDirty) OnControlsChanged();
    }

    void OnControlsChanged()
    {
        if (_playerInput != null)
        {
            _controlSchemeChanged.Invoke(_playerInput.currentControlScheme);
            CurrentScheme = _playerInput.currentControlScheme;
            _isDirty = false;
        }
        else _isDirty = true;
    }
}
