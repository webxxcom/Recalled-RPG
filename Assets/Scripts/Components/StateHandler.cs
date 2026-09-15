using System;
using UnityEngine;

public class StateHandler : ToggleableState
{
    [SerializeField] GameState _definition;
    [SerializeField] VoidGameEvent OnGameEventRaised;
    [SerializeField] UIScreen _uiScreen;

    public GameState Definition => _definition;

    public override event Action<ToggleableState> Toggled;

    void Raise() => Toggled?.Invoke(this);
    void OnEnable()
        => OnGameEventRaised.OnEventRaised += Raise;
    void OnDisable()
        => OnGameEventRaised.OnEventRaised -= Raise;

    protected override void Activate()
    {
        _uiScreen.IsActive = true;
    }
    protected override void Deactivate()
    {
        _uiScreen.IsActive = false;
    }
}
