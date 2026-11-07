using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(PrefabPopulator))]
    internal sealed class InventoryItemsView : MonoBehaviour
    {
        [SerializeField] InventoryAnchor _inventoryAnchor;
        [SerializeField] ItemCategory _currentFilter = ItemCategory.Any;

        [SerializeField] SlotType _displayedSlot;
        PrefabPopulator _populator;
        IReadOnlyList<EquipmentSlot> _itemsView;
        bool _isDirty;

        private void Awake()
        {
            _populator = GetComponent<PrefabPopulator>();
        }

        void OnEnable()
        {
            _inventoryAnchor.Provided += OnInventoryProvided;

            if (_inventoryAnchor.IsSet) OnInventoryProvided(_inventoryAnchor.Value);
        }
        void OnDisable() => _inventoryAnchor.Provided -= OnInventoryProvided;

        public void Filter(SlotType slot)
        {
            if (_displayedSlot == slot)
                return;

            _displayedSlot = slot;
            _isDirty = true;
        }

        void OnInventoryProvided(IBagView inventory)
        {
            inventory.Changed += () => _isDirty = true;
            _isDirty = true;
        }

        public void RefreshView()
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
