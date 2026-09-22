using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsView : MonoBehaviour
{
    [SerializeField] InventoryItemsListSO _inventoryItems;
    [SerializeField] bool _isPopulated;

    /// <summary>View can be populated with the slots by slots creator</summary>
    IReadOnlyList<InventorySlot> _slots;
    public IReadOnlyList<InventorySlot> Slots
    {
        get => _slots;
        set
        {
            _slots = value;
            RefreshView();
        }
    }
    public IReadOnlyList<ItemInstance> Items => _inventoryItems.Items;

    ItemCategory _currentFilter = ItemCategory.Any;
    public ItemCategory Filter
    {
        get => _currentFilter;
        set
        {
            if (value == _currentFilter)
                return;

            _currentFilter = value;
            RefreshView();
        }
    }

    private void Awake()
    {
        if (!_isPopulated) _slots = GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        _inventoryItems.ItemsChanged += RefreshView;

        RefreshView();
    }

    private void OnDisable()
    {
        _inventoryItems.ItemsChanged -= RefreshView;
    }

    public void RefreshView()
    {
        if (Slots == null)
            return;

        var items = _inventoryItems.Items;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < items.Count && !items[i].IsEmpty
                && (Filter == ItemCategory.Any || items[i].Definition.Category == Filter))
                Slots[i].SetItem(items[i]);
            else
                Slots[i].RemoveItem();
        }
    }
}
