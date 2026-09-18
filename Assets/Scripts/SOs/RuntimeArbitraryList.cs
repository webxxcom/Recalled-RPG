using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeArbitraryList : ScriptableObject
{
    [SerializeField] protected ItemInstance[] _items;

    public IReadOnlyList<ItemInstance> Items => _items;
    public event Action ItemsChanged;

    public void VisualsChanged()
    {
        ItemsChanged?.Invoke();
    }
}
