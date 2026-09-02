using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    readonly List<T> _items = new();

    public List<T> Items => _items;

    public event Action OnChanged;

    public void Add(T obj)
    {
        if (_items.Contains(obj)) return;

        _items.Add(obj);
        OnChanged?.Invoke();
    }

    public void Remove(T obj)
    {
        if (!_items.Remove(obj)) return;

        OnChanged?.Invoke();
    }

}
