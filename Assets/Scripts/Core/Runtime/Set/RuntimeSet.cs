using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    readonly HashSet<T> _items = new();

    public IReadOnlyCollection<T> Items => _items;

    public event Action OnChanged;

    public void Add(T obj)
    {
        if (!_items.Add(obj))
            return;

        OnChanged?.Invoke();
    }

    public void Remove(T obj)
    {
        if (!_items.Remove(obj))
            return;

        OnChanged?.Invoke();
    }
}
