using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public readonly struct HealthValue : IEquatable<HealthValue>
    {
        public int Current { get; }
        public int Max { get; }

        public HealthValue(int current, int max)
        {
            Max = Mathf.Max(0, max);
            Current = Mathf.Clamp(current, 0, Max);
        }

        public float Normalized => Max > 0 ? (float)Current / Max : 0f;

        public HealthValue WithCurrent(int current) => new(current, Max);
        public HealthValue WithMax(int max) => new(Current, max);

        public bool Equals(HealthValue other) => Current == other.Current && Max == other.Max;
        public override bool Equals(object obj) => obj is HealthValue other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Current, Max);
    }
}
