using System;
using UnityEngine;

namespace Recalled.Core
{
    [CreateAssetMenu(menuName = "Runtime/Event Channels/Void")]
    public sealed class VoidGameEvent : ScriptableObject
    {
        public event Action EventRaised;

        public void AddListener(Action list) => EventRaised += list;
        public void RemoveListener(Action list) => EventRaised -= list;
        public void Invoke() => EventRaised?.Invoke();
    }
}
