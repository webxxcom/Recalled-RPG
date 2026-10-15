using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    readonly HashSet<T> _items = new();

    public IReadOnlyCollection<T> Items => _items;

    public event Action Changed;

    public void Add(T obj)
    {
        if (!_items.Add(obj))
            return;

        Changed?.Invoke();
    }

    public void Remove(T obj)
    {
        if (!_items.Remove(obj))
            return;

        Changed?.Invoke();
    }
}
