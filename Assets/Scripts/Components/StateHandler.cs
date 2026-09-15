using System;
using UnityEngine;

public class StateHandler : ToggleableScreen
{
    [SerializeField] GameState _definition;
    [SerializeField] VoidGameEvent OnGameEventRaised;
    [SerializeField] UIScreen _uiScreen;

    public GameState Definition => _definition;

    public override event Action<ToggleableScreen> Toggled;

    void Raise() => Toggled?.Invoke(this);
    void OnEnable()
        => OnGameEventRaised.OnEventRaised += Raise;
    void OnDisable()
        => OnGameEventRaised.OnEventRaised -= Raise;

    protected override void Show()
    {
        _uiScreen.IsOpen = true;
    }
    protected override void Hide()
    {
        _uiScreen.IsOpen = false;
    }
}
