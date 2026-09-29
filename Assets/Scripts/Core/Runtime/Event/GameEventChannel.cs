using System;
using UnityEngine;

namespace Recalled.Core
{
    public class GameEventChannel<T> : ScriptableObject
    {
        public event Action<T> EventRaised;

        public void Invoke(T payload) => EventRaised?.Invoke(payload);
        public void AddListener(Action<T> action) => EventRaised += action;
        public void RemoveListener(Action<T> action) => EventRaised -= action;
    }
}
