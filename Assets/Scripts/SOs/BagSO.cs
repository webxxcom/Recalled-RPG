using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Collection")]
public class BagSO : RuntimeArbitraryList
{
    [SerializeField, Min(1), Delayed] int _maxItemsCount;

    public bool IsFull => !_items.Contains(null);
    public int ItemLimit => _maxItemsCount;

    bool AddNewItem(ItemInstance item)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (!_items[i].IsEmpty)
                continue;

            // Found empty spot then we can assign
            _items[i].Definition = item.Definition;
            _items[i].Count = item.Count;
            VisualsChanged();
            return true;
        }

        return false;
    }

    /// <summary>Either adds more stocks to the <see cref="ItemInstance.Count"/> or adds brand new item depending on <see cref="ItemDefinition.MaxStockSize"/></summary>
    public bool Add(ItemInstance itemInstance)
    {
        if (itemInstance == null || itemInstance.IsEmpty || _items.Contains(itemInstance)) return false;

        // Optimize a little not trying to add instackable item
        if (itemInstance.Definition.IsStackable)
        {
            int count = itemInstance.Count;
            foreach (var item in _items)
            {
                if (item.Definition != itemInstance.Definition)
                    continue;

                // If we'll overflow the max stock count then current item count becomes the difference
                if (item.Count + count > item.Definition.MaxStockSize)
                {
                    count -= (item.Definition.MaxStockSize - item.Count);
                    item.Count = item.Definition.MaxStockSize;
                }
                else
                {
                    // Guard it just in case
                    item.Count = Mathf.Clamp(item.Count + itemInstance.Count, 1, item.Definition.MaxStockSize);
                    return true;
                }
            }
        }

        // We're either left with some Count on the item or no match was found so we're adding new item if we can
        return AddNewItem(itemInstance);
    }

    bool RemoveItem(int ind)
    {
        _items[ind].SetEmpty();
        VisualsChanged();
        return true;
    }

    /// <summary>Remove this exact item instance from the inventory</summary>
    public bool Remove(ItemInstance itemInstance)
    {
        if (itemInstance == null) return false;

        for (int i = 0; i < _items.Length; ++i)
        {
            if (_items[i] != itemInstance)
                continue;

            return RemoveItem(i);
        }

        return false;
    }
    public bool Remove(ItemDefinition itemDefinition)
    {
        if (itemDefinition == null) return false;

        for (int i = 0; i < _items.Length; i++)
            if (_items[i].Definition == itemDefinition) return RemoveItem(i);

        return false;
    }

    bool TakeRecursively(ItemDefinition definition, int count, int i)
    {
        for (; i < _items.Length; i++)
        {
            if (_items[i].Definition != definition)
                continue;

            int newCount = _items[i].Count - count;
            if (newCount < definition.MinStockSize)
            {
                if (!TakeRecursively(definition, -newCount, i + 1))
                    return false;

                _items[i].Count = definition.MinStockSize;
                return true;
            }
            else
            {
                _items[i].Count = newCount;
                return true;
            }
        }
        return false;
    }

    public bool Take(ItemDefinition definition, int count)
    {
        if (count < 1 || count > 999 || definition == null) return false;

        if (TakeRecursively(definition, count, 0))
        {
            VisualsChanged();
            return true;
        }
        return false;
    }

    public bool Has(ItemDefinition itemDefinition)
        => itemDefinition != null && (_items.FirstOrDefault(item => item?.Definition == itemDefinition) != null);

#if UNITY_EDITOR
    int _prevCount;
    private void OnValidate()
    {
        if (_prevCount != _maxItemsCount)
        {
            _prevCount = _maxItemsCount;

            var oldItems = _items;
            _items = new ItemInstance[_maxItemsCount];

            int i = 0;
            for (; i < Mathf.Min(_items.Length, oldItems.Length); i++)
                _items[i] = oldItems[i];
        }
    }
#endif
}
