using System;
using UnityEngine;

public abstract class RuntimeVariable<T> : ScriptableObject
{
    [SerializeField] T _value;
    [SerializeField, HideInInspector] bool _wasInit = false;

    public T Value
    {
        get => _value;
        set
        {
            if (_value != null && _value.Equals(value))
                return;

            _value = value;
            _wasInit = true;
            ValueChanged?.Invoke(_value);
        }
    }

    event Action<T> ValueChanged;

    public void Subscribe(Action<T> action)
    {
        if (_wasInit)
            action.Invoke(Value);

        ValueChanged += action;
    }

    public void Unsubscribe(Action<T> action)
    {
        ValueChanged -= action;
    }

#if UNITY_EDITOR
    [SerializeField, HideInInspector] T _prev;
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