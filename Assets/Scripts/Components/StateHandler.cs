using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField] StateStackHandler _stateManager;
    [SerializeField] GameState _definition;
    [SerializeField] VoidGameEvent OnGameEventRaised;
    [SerializeField] UIScreen _uIScreen;

    public bool IsActive
    {
        get => _uIScreen.IsActive;
        set
        {
            if (value == IsActive)
                return;

            if (value) Show();
            else Hide();
        }
    }

    void OnEnable()
        => OnGameEventRaised.OnEventRaised += Toggle;
    void OnDisable()
        => OnGameEventRaised.OnEventRaised -= Toggle;
    void Toggle() 
        => IsActive = !IsActive;

    void Show()
    {
        if (_stateManager.Add(_definition))
            _uIScreen.IsActive = true;
    }
    void Hide()
    {
        if (_stateManager.Remove(_definition))
            _uIScreen.IsActive = false;
    }
}
