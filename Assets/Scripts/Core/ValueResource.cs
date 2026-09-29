using System;
using UnityEngine;

public abstract class ValueResource : MonoBehaviour
{
    [SerializeField] ValueProviderConfig _config;

    [SerializeField] int _maxValue;
    [Tooltip("SetField the value variable in the config")]
    [SerializeField] IntVariable _currentValue;
    [SerializeField] bool _isInfinite;

    public int MaxValue => _maxValue;
    public int CurrentValue => _currentValue.Value;

    /// <summary> (oldVal, newVal) after the change </summary>
    public event Action<int, int> OnValueChanged;
    public event Action<int> OnMinValue;
    public event Action<int> OnMaxValue;

    protected virtual void Awake()
    {
        //TODO ???
        //_maxValue = _config.MaximumValue;
        //_currentValue = _config.CurrentValue;
        //_isStatic = _config.IsStatic;

        _currentValue.Value = MaxValue;
    }

    /// <returns>The delta actually applied, after clamping</returns>
    public int Replenish(int delta)
    {
        if (delta == 0)
            return 0;

        int oldVal = _currentValue.Value;
        int newVal = Mathf.Clamp(_currentValue.Value + delta, 0, _maxValue);
        if (!_isInfinite)
            _currentValue.Value = newVal;

        int applied = oldVal - newVal;
        if (applied == 0)
            return 0;

        OnValueChanged?.Invoke(oldVal, newVal);

        if (oldVal != 0 && newVal == 0)
            OnMinValue?.Invoke(oldVal);
        else if (oldVal != MaxValue && newVal == _maxValue)
            OnMaxValue?.Invoke(oldVal);

        return applied;
    }
    public int Consume(int amount) => Replenish(-amount);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_config != null)
        {
            if (_config.CurrentValue == null) // CurrentValue is absent - it's not set in the config - create it
                _currentValue = ScriptableObject.CreateInstance<IntVariable>();
            else // The Variable is set for bosses and Player
                _currentValue = _config.CurrentValue;

            _maxValue = _config.MaximumValue;
            _currentValue.Value = _maxValue;
            _isInfinite = _config.IsInfinite;
        }
    }
#endif
}
