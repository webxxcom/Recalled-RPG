using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Collection")]
public class BagSO : RuntimeArbitraryList
{
    public int MaxItemsCount => _items.Length;

    bool AddNewItem(ItemDefinition definition, int count)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (!_items[i].IsEmpty)
                continue;

            // Found empty spot then we can assign
            _items[i].SetItem(definition, count);
            ContentChanged();
            return true;
        }

        // Didn't add new item because we're full
        return false;
    }

    bool TryAddStockedRecursive(ItemInstance itemInstance, int countToAdd, int i)
    {
        for (; i < _items.Length; ++i)
        {
            ItemInstance item = _items[i];
            if (item.Definition != itemInstance.Definition)
                continue;

            // If overflows the max stock count then current item count becomes the difference
            if (item.Count + countToAdd > item.Definition.MaxStockSize)
            {
                countToAdd -= (item.Definition.MaxStockSize - item.Count);

                if (!TryAddStockedRecursive(itemInstance, countToAdd, i + 1))
                    return false;

                item.Count = item.Definition.MaxStockSize;
                return true;
            }
            else
            {
                item.Count += countToAdd;
                return true;
            }
        }
        return false;
    }

    bool TryAddStocked(ItemInstance itemInstance)
    {
        if (TryAddStockedRecursive(itemInstance, itemInstance.Count, 0))
        {
            ContentChanged();
            return true;
        }
        return false;
    }

    /// <summary>Either adds more stocks to the <see cref="ItemInstance.Count"/> or adds brand new item depending on <see cref="ItemDefinition.MaxStockSize"/></summary>
    public bool Add(ItemInstance itemInstance)
    {
        if (itemInstance == null || itemInstance.IsEmpty || _items.Contains(itemInstance))
            return false;

        // Optimize a little not trying to add and unstackable item
        if (itemInstance.Definition.IsStackable && TryAddStocked(itemInstance))
            return true;

        // We're either left with some Count on the item or no match was found so we're adding new item if we can
        return AddNewItem(itemInstance.Definition, itemInstance.Count);
    }

    bool RemoveItem(int ind)
    {
        _items[ind].SetEmpty();
        ContentChanged();
        return true;
    }

    /// <summary>Remove this exact item instance from the inventory</summary>
    public bool Remove(ItemInstance itemInstance)
    {
        if (itemInstance == null)
            return false;

        for (int i = 0; i < _items.Length; ++i)
        {
            if (_items[i] != itemInstance)
                continue;

            return RemoveItem(i);
        }

        return false;
    }
    public bool Remove(ItemDefinition itemDefinition)
        => Take(itemDefinition, 1);

    bool TakeRecursively(ItemDefinition definition, int count, int i)
    {
        for (; i < _items.Length; i++)
        {
            if (_items[i].Definition != definition)
                continue;

            int newCount = _items[i].Count - count;
            if (newCount == 0)
                return RemoveItem(i);
            else if (newCount < definition.MinStockSize)
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
        if (count < 0 || definition == null) return false;

        if (TakeRecursively(definition, count, 0))
        {
            ContentChanged();
            return true;
        }
        return false;
    }

    public bool Has(ItemDefinition itemDefinition)
        => itemDefinition != null && (_items.FirstOrDefault(item => item?.Definition == itemDefinition) != null);
}
