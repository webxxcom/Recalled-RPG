using System;
using UnityEngine;

namespace Recalled.Core
{
    public sealed class ValueResource
    {
        public int MinValue => 0;
        public int MaxValue { get; private set; }
        public int Current { get; private set; }
        public bool IsInfinite { get; private set; }

        /// <summary> (oldVal, newVal) after the change </summary>
        public event Action<int, int> ValueChanged;
        public event Action<int, int> ValueApplied;

        public ValueResource(int current, int maxValue, bool isInfinite)
        {
            Current = current;
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

            int oldVal = Current;
            int newVal = Mathf.Clamp(Current + delta, MinValue, MaxValue);
            if (!IsInfinite)
                Current = newVal;

            int applied = oldVal - newVal;
            if (applied == 0)
                return 0;

            ValueChanged?.Invoke(oldVal, newVal);
            ValueApplied?.Invoke(oldVal, Current);

            return applied;
        }
        public int Consume(int amount) => Replenish(-amount);

        public bool Has(int amount) => IsInfinite || Current <= amount;
    }
}
