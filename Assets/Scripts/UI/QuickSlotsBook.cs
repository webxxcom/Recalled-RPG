using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuickSlotsBook : MonoBehaviour
{
    [SerializeField] InventorySO _inventory;

    IReadOnlyList<InventoryCell> _slots;

    public bool IsFull => _slots.Count(c => c.HasItem) >= _slots.Count;

    private void Awake()
    {
        _slots = GetComponentsInChildren<InventoryCell>();
    }

    private void OnEnable()
    {
        _inventory.OnItemsChanged += RefreshView;

        RefreshView();
    }

    private void OnDisable()
    {
        _inventory.OnItemsChanged -= RefreshView;
    }

    /// <summary>Tries to find an available quick slot, returns false if fails</summary>
    public InventoryCell Add(ItemInstance item)
    {
        foreach (var slot in _slots)
        {
            if (!slot.HasItem)
            {
                slot.SetItem(item);
                return slot;
            }
        }
        return null;
    }

    public ItemInstance RemoveItemFromCell(InventoryCell cell)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i] == cell)
            {
                var removed = _slots[i].Item;

                while (i + 1 < _slots.Count && _slots[i + 1].HasItem)
                    _slots[i].SetItem(_slots[i++].Item);
                _slots[i].RemoveItem();

                return removed;
            }
        }
        return null;
    }

    public void RefreshView()
    {
        var quickItems = _inventory.Items.Except(_inventory.GeneralItems).ToArray();
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < quickItems.Length)
                _slots[i].SetItem(quickItems[i]);
            else _slots[i].RemoveItem();
        }
    }

}
