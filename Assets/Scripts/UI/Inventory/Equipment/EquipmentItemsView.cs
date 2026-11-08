using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class EquipmentItemsView : MonoBehaviour
    {
        [SerializeField] SlotType _displayedSlot;
        [SerializeField] EquipmentAnchor _equipmentAnchor;
        [SerializeField] PrefabPopulator _populator;

        bool _isDirty;

        private void OnEnable()
        {
            _equipmentAnchor.Provided += OnAnchorProvided;

            if (_equipmentAnchor.IsSet) OnAnchorProvided(_equipmentAnchor.Value);
        }
        private void OnDisable()
        {
            _equipmentAnchor.Provided -= OnAnchorProvided;
        }

        void OnAnchorProvided(Equipment equipment)
        {
            equipment.Changed += _ => _isDirty = true;
        }

        public void Filter(SlotType slot)
        {
            if (_displayedSlot == slot)
                return;

            _displayedSlot = slot;
            _isDirty = true;
        }

        public void RefreshView()
        {
            if (!_equipmentAnchor.IsSet)
                return;

            _populator.DestroyAll();
            var equippableItems = _equipmentAnchor.Value.EquippableItems;
            for (int i = 0; i < equippableItems.Count; i++)
            {
                var itemSlot = equippableItems[i];

                if (_displayedSlot != itemSlot.Equippable.Slot)
                    continue;

                var addedSlot = _populator.Add().GetComponent<EquipmentSlot>();
                addedSlot.ShowItem(itemSlot);
            }
        }

        private void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;
                RefreshView();
            }
        }
    }
}
