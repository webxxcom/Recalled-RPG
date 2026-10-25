using System;
using UnityEngine;

namespace Recalled.Core
{
    public abstract class RuntimeAnchor<T> : ScriptableObject where T : class
    {
        public T Value { get; private set; }
        public bool IsSet => Value != null;

        public event Action<T> Provided;

        public void Provide(T value)
        {
            if (IsSet || value == null || ReferenceEquals(Value, value))
                return;

            Value = value;
            Provided?.Invoke(value);
        }
        public void Withdraw(T value)
        {
            if (!ReferenceEquals(value, Value))
                return;

            Value = null;
        }
    }
}
