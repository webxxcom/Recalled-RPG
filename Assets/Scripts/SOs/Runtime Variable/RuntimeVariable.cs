using System;
using UnityEngine;

public abstract class RuntimeVariable<T> : ScriptableObject
{
    [SerializeField] T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (_value != null && _value.Equals(value))
                return;

            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public event Action<T> OnValueChanged;

#if UNITY_EDITOR
    T _prev;
    private void OnValidate()
    {
        if (_prev != null && !_prev.Equals(_value))
        {
            Value = _value;
            _prev = _value;
        }
    }
#endif
}