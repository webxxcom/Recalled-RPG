using System;
using UnityEngine;

public abstract class ValueResource : MonoBehaviour
{
    [SerializeField] ValueProviderConfig _config;

    [Tooltip("Optional Runtime Variable, mostly for player and bosses")]
    [SerializeField] IntVariable _intVariable;

    public int MinValue => 0;
    public int MaxValue { get; private set; }
    public int CurrentValue { get; private set; }
    public bool IsInfinite { get; private set; }

    /// <summary> (oldVal, newVal) after the change </summary>
    public event Action<int, int> ValueChanged;
    public event Action<int> MinValueReached;
    public event Action<int> MaxValueReached;

    protected virtual void Awake()
    {
        MaxValue = _config.MaximumValue;
        CurrentValue = _config.InitValue;
        IsInfinite = _config.IsInfinite;

        if (_intVariable)
            _intVariable.Value = CurrentValue;
    }

    /// <returns>The delta actually applied, after clamping</returns>
    public int Replenish(int delta)
    {
        if (delta == 0)
            return 0;

        int oldVal = CurrentValue;
        int newVal = Mathf.Clamp(CurrentValue + delta, MinValue, MaxValue);
        if (!IsInfinite)
            CurrentValue = newVal;
        if (_intVariable)
            _intVariable.Value = CurrentValue;

        int applied = oldVal - newVal;
        if (applied == 0)
            return 0;

        ValueChanged?.Invoke(oldVal, newVal);

        if (oldVal != 0 && newVal == 0)
            MinValueReached?.Invoke(oldVal);
        else if (oldVal != MaxValue && newVal == MaxValue)
            MaxValueReached?.Invoke(oldVal);

        return applied;
    }
    public int Consume(int amount) => Replenish(-amount);
}
