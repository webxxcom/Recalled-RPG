using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySlotsView : MonoBehaviour
{
    [SerializeField] InventoryItemsListSO _inventoryItems;
    [SerializeField] PrefabPopulator _populator;

    IReadOnlyList<InventorySlot> _slots;
    ItemCategory _currentFilter = ItemCategory.Any;

    private void Awake()
    {
        _slots = _populator != null
            ? _populator.Populate(_inventoryItems.Items.Count).Select(go => go.GetComponent<InventorySlot>()).ToArray()
            : GetComponentsInChildren<InventorySlot>();
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

    public void Filter(ItemCategory filter)
    {
        if (filter == _currentFilter)
            return;

        _currentFilter = filter;
        RefreshView();
    }

    public void RefreshView()
    {
        if (_slots == null)
            throw new InvalidOperationException();

        var items = _inventoryItems.Items;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count && !items[i].IsEmpty
                && (_currentFilter == ItemCategory.Any || items[i].Definition.Category == _currentFilter))
                _slots[i].SetItem(items[i]);
            else
                _slots[i].RemoveItem();
        }
    }
}
