using UnityEngine;

public sealed class GameStateScreen : MonoBehaviour
{
    [SerializeField] GameStateStack _stack;
    [SerializeField] Toggleable _screen;
    [SerializeField] GameStateSO _state;
    
    bool _started;

    void Awake()
    {
        if (_stack == null || _screen == null)
        {
            Debug.LogError($"{nameof(GameStateScreen)}: missing references.", this);
            enabled = false;
        }
    }

    void OnEnable()
    {
        _stack.Changed += Sync;

        if (_started) Sync();
    }
    void OnDisable()
    {
        _stack.Changed -= Sync;
    }

    void Start()
    {
        _started = true;
        Sync();
    }

    void Sync()
    {
        bool visible = _stack.CurrentState == _state;

        ((IToggleable)_screen).SetIsActive(visible);
    }
}