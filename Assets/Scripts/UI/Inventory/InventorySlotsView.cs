using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class InventorySlotsView : UiView<IBagView>
    {
        [SerializeField] InventoryAnchor _inventoryAnchor;

        IReadOnlyList<InventorySlot> _slotsView;
        ItemCategory _currentFilter = ItemCategory.Any;
        protected override RuntimeAnchor<IBagView> Anchor => _inventoryAnchor;

        public void Filter(ItemCategory filter)
        {
            if (filter == _currentFilter)
                return;

            _currentFilter = filter;
            _isDirty = true;
        }

        protected override void Refresh()
        {
            var inventory = _inventoryAnchor.Value;
            _slotsView ??= _populator != null
                ? _populator.Populate(inventory.Count).Select(go => go.GetComponent<InventorySlot>()).ToArray()
                : GetComponentsInChildren<InventorySlot>();

            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];
                var slotEnabled = item != null && item.Definition.IsInCategory(_currentFilter);

                if (item != null) _slotsView[i].ShowItem(item, !slotEnabled);
                else _slotsView[i].ShowEmpty();
            }
        }
    }
}
