using Recalled.Gameplay;
using Recalled.Systems.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySlotsView : MonoBehaviour
{
    [SerializeField] InventoryVariable _inventoryVariable;
    [SerializeField] PrefabPopulator _populator;

    ItemSlotsArray _inventory;
    IReadOnlyList<InventorySlotView> _slotsView;
    ItemCategory _currentFilter = ItemCategory.Any;

    private void OnEnable()
    {
        _inventoryVariable.ValueChanged += OnInventorySet;
    }

    private void OnDisable()
    {
        _inventoryVariable.ValueChanged -= OnInventorySet;
    }

    void OnInventorySet(ItemSlotsArray inventory)
    {
        if (_inventory != null) _inventory.SlotsChanged -= RefreshView;

        _inventory = inventory;
        _inventory.SlotsChanged += RefreshView;

        RefreshView();
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
        // Lazy init
        _slotsView ??= _populator != null
            ? _populator.Populate(_inventoryVariable.Value.Count).Select(go => go.GetComponent<InventorySlotView>()).ToArray()
            : GetComponentsInChildren<InventorySlotView>();

        for (int i = 0; i < _inventoryVariable.Value.Count; i++)
        {
            var slot = _inventoryVariable.Value[i];
            if (!slot.IsEmpty && (_currentFilter == ItemCategory.Any || slot.Item.Definition.Category == _currentFilter))
                _slotsView[i].SetItem(slot.Item);
            else
                _slotsView[i].RemoveItem();
        }
    }
}
