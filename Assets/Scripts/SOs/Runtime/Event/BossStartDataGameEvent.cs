using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/BossData Game Event")]
public class BossStartDataGameEvent : ScriptableObject
{
    public event Action<BossData> OnEventRaised;

    public void Invoke(BossData bsd) => OnEventRaised?.Invoke(bsd);
}
