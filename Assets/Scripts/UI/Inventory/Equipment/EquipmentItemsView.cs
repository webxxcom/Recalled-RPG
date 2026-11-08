using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System.Linq;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class EquipmentItemsView : UiView<Equipment>
    {
        [SerializeField] SlotType _displayedSlot;
        [SerializeField] EquipmentAnchor _equipmentAnchor;

        protected override RuntimeAnchor<Equipment> Anchor => _equipmentAnchor;

        public void Filter(SlotType slot)
        {
            if (_displayedSlot == slot)
                return;

            _displayedSlot = slot;
            _isDirty = true;
        }

        protected override void Refresh()
        {
            if (!_equipmentAnchor.IsSet)
                return;

            int j = 0;
            var equippableItems = _equipmentAnchor.Value.EquippableItems;
            foreach (var itemSlot in equippableItems.Where(ei => ei.Equippable.Slot == _displayedSlot))
            {
                if (j >= _populator.Created.Count) _populator.Add();

                var addedSlot = _populator.Created[j++].GetComponent<EquipmentSlot>();
                addedSlot.ShowItem(itemSlot);
            }
            _populator.RemoveAfterAt(j);
        }
    }
}
