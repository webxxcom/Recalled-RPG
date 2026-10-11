using System;
using UnityEngine;

namespace Recalled.Core
{
    public sealed class ValueResource
    {
        public int MinValue => 0;
        public int MaxValue { get; private set; }
        public int CurrentValue { get; private set; }
        public bool IsInfinite { get; private set; }

        /// <summary> (oldVal, newVal) after the change </summary>
        public event Action<int, int> ValueChanged;

        public ValueResource(int currentValue, int maxValue, bool isInfinite)
        {
            CurrentValue = currentValue;
            MaxValue = maxValue;
            IsInfinite = isInfinite;
        }

        public ValueResource(ValueProviderConfig config)
            : this(config.InitValue, config.MaximumValue, config.IsInfinite) { }

        /// <returns>The delta actually applied, after clamping</returns>
        public int Replenish(int delta)
        {
            if (delta == 0)
                return 0;

            int oldVal = CurrentValue;
            int newVal = Mathf.Clamp(CurrentValue + delta, MinValue, MaxValue);
            if (!IsInfinite)
                CurrentValue = newVal;

            int applied = oldVal - newVal;
            if (applied == 0)
                return 0;

            ValueChanged?.Invoke(oldVal, newVal);

            return applied;
        }
        public int Consume(int amount) => Replenish(-amount);

        public bool Has(int amount) => IsInfinite || CurrentValue <= amount;
    }
}
