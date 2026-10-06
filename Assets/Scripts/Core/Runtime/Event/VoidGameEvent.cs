using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Game Event")]
public class VoidGameEvent : ScriptableObject
{
    public event Action OnEventRaised;

    public void AddListener(Action list) => OnEventRaised += list;
    public void RemoveListener(Action list) => OnEventRaised -= list;
    public void Invoke() => OnEventRaised?.Invoke();
}
