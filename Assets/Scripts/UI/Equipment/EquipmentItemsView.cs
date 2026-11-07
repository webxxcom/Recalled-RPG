using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class EquipmentItemsView : InventoryItemsView
    {
        [SerializeField] SlotType _displayedSlot;

        public void Filter(SlotType slot)
        {
            if (_displayedSlot == slot)
                return;

            _displayedSlot = slot;
            _isDirty = true;
        }

        public override void RefreshView()
        {
            if (!_inventoryAnchor.IsSet)
                return;

            _populator.DestroyAll();
            var equippableItems = _inventoryAnchor.Value.Slots.Where(s => !s.IsEmpty && s.Item.Definition.IsInCategory(ItemCategory.Equipment)).ToArray();
            for (int i = 0; i < equippableItems.Length; i++)
            {
                var itemSlot = equippableItems[i];

                if (!itemSlot.Item.Definition.TryGetModule<EquippableModule>(out var module) || _displayedSlot != module.Slot)
                    continue;

                var addedSlot = _populator.Add().GetComponent<EquipmentSlot>();
                addedSlot.ShowItem(itemSlot);
            }
        }
    }
}
