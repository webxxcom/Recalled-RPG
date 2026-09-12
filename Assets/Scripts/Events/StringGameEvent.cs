using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/String Game Event")]
public class StringGameEvent : ScriptableObject
{
    event UnityAction<string> OnEventRaised;

    public void Invoke(string str) => OnEventRaised?.Invoke(str);
    public void AddListener(UnityAction<string> listener) => OnEventRaised += listener;
    public void RemoveListener(UnityAction<string> listener) => OnEventRaised -= listener;
}
