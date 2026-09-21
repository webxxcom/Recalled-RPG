using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeArbitraryList : ScriptableObject
{
    [SerializeField] protected ItemInstance[] _items;

    public IReadOnlyList<ItemInstance> Items => _items;
    public event Action ItemsChanged;

    /// <summary>
    /// Function called in child classes which simply invokes the event <see cref="RuntimeArbitraryList.ItemsChanged"/>
    /// </summary>
    protected void ContentChanged()
    {
        ItemsChanged?.Invoke();
    }
}
