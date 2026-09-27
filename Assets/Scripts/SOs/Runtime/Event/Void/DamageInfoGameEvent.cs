using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Damage Info Game Event")]
public class DamageInfoGameEvent : ScriptableObject
{
    private readonly List<IDamageInfoGameEventListener> listeners = new();

    public void Raise(DamageInfo di)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(di);
    }

    public void AddListener(IDamageInfoGameEventListener listener) => listeners.Add(listener);
    public void RemoveListener(IDamageInfoGameEventListener listener) => listeners.Remove(listener);
}
