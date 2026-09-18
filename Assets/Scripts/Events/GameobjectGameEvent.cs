using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Gameobject Game Event")]
public class GameobjectGameEvent : ScriptableObject
{
    public event Action<GameObject> OnEventRaised;

    public void Invoke(GameObject game) => OnEventRaised?.Invoke(game);
    public void AddListener(Action<GameObject> listener) => OnEventRaised += listener;
    public void RemoveListener(Action<GameObject> listener) => OnEventRaised -= listener;
}
