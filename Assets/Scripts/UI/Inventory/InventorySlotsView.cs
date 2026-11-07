using Recalled.Systems.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    internal sealed class InventorySlotsView : InventoryItemsView
    {
        IReadOnlyList<InventorySlot> _slotsView;
        ItemCategory _currentFilter = ItemCategory.Any;

        public void Filter(ItemCategory filter)
        {
            if (filter == _currentFilter)
                return;

            _currentFilter = filter;
            _isDirty = true;
        }

        public override void RefreshView()
        {
            if (!_inventoryAnchor.IsSet)
                return;

            // Lazy init
            _slotsView ??= _populator != null
                ? _populator.Populate(_inventoryAnchor.Value.Count).Select(go => go.GetComponent<InventorySlot>()).ToArray()
                : GetComponentsInChildren<InventorySlot>();

            if (_slotsView.Count != _inventoryAnchor.Value.Count)
            {
                Debug.LogError($"{name}: The {nameof(_slotsView)}'s Amount doesn't match the {nameof(_inventoryAnchor.Value)}'s Amount");
                return;
            }

            InventorySlot firstActiveSlot = default;
            for (int i = 0; i < _inventoryAnchor.Value.Count; i++)
            {
                var item = _inventoryAnchor.Value[i];
                var isVisible = item != null && item.Definition.IsInCategory(_currentFilter);

                if (item != null) _slotsView[i].ShowItem(item, !isVisible);
                else _slotsView[i].ShowEmpty();

                if (firstActiveSlot == null && isVisible) firstActiveSlot = _slotsView[i];
            }

            var currentSel = EventSystem.current.currentSelectedGameObject;
            if (firstActiveSlot != null)
            {
                if (currentSel == null || (currentSel.TryGetComponent<Selectable>(out var s) && !s.enabled))
                    EventSystem.current.SetSelectedGameObject(firstActiveSlot.gameObject);
            }
            else EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
