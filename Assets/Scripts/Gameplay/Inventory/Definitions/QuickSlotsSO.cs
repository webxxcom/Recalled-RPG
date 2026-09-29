using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory/quick slots")]
public class QuickSlotsSO : InventoryItemsListSO
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ind"></param>
    /// <param name="itemInstance"></param>
    /// <param name="replaced">In case of replacing some item</param>
    /// <returns>Flag indicating if an item was removed from the slots</returns>
    public bool TrySet(int ind, ItemInstance itemInstance, out ItemInstance replaced)
    {
        replaced = new();
        if (ind >= _items.Length || itemInstance == null || itemInstance.IsEmpty)
            return false;

        ItemInstance newItem = new(itemInstance.Definition, itemInstance.Count);

        // If this item is already ours we just switch places
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != itemInstance)
                continue;

            // Do nothing it it's the same position
            if (i == ind) return false;

            _items[i].SetEmpty();
        }

        if (!_items[ind].IsEmpty)
            replaced = new(_items[ind].Definition, _items[ind].Count);

        _items[ind] = newItem;
        ContentChanged();
        return true;
    }

    public bool Set(Vector2 vec2, ItemInstance itemInstance, out ItemInstance replaced)
    {
        if (vec2 == Vector2.left) return TrySet(0, itemInstance, out replaced);
        if (vec2 == Vector2.up) return TrySet(1, itemInstance, out replaced);
        if (vec2 == Vector2.right) return TrySet(2, itemInstance, out replaced);
        if (vec2 == Vector2.down) return TrySet(3, itemInstance, out replaced);

        replaced = new();
        return false;
    }

    public bool UnSet(ItemInstance itemInstance)
    {
        if (itemInstance == null || itemInstance.IsEmpty || !_items.Contains(itemInstance))
            return false;

        foreach (var slot in _items)
        {
            if (slot != itemInstance)
                continue;

            slot.SetEmpty();
            ContentChanged();
            return true;
        }
        return false;
    }
}
