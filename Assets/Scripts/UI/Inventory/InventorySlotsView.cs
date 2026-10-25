using Recalled.Gameplay;
using Recalled.Systems.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    public sealed class InventorySlotsView : MonoBehaviour
    {
        [SerializeField] InventoryVariable _inventoryVariable;
        [SerializeField] PrefabPopulator _populator;

        ItemSlotsArray _inventory;
        IReadOnlyList<InventorySlot> _slotsView;
        ItemCategory _currentFilter = ItemCategory.Any;

        private void Awake()
        {
            _inventoryVariable.Subscribe(OnInventorySet);
        }

        private void OnEnable()
        {
            RefreshView();
        }

        private void OnDestroy()
        {
            _inventoryVariable.Unsubscribe(OnInventorySet);
        }

        void OnInventorySet(ItemSlotsArray inventory)
        {
            if (_inventory != null) _inventory.SlotsChanged -= RefreshView;

            _inventory = inventory;
            if (_inventory != null)
            {
                _inventory.SlotsChanged += RefreshView;

                RefreshView();
            }
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
            if (_inventory == null)
                return;

            // Lazy init
            _slotsView ??= _populator != null
                ? _populator.Populate(_inventory.Count).Select(go => go.GetComponent<InventorySlot>()).ToArray()
                : GetComponentsInChildren<InventorySlot>();

            if (_slotsView.Count != _inventory.Count)
            {
                Debug.LogError($"{name}: The {nameof(_slotsView)}'s Count doesn't match the {nameof(_inventory)}'s Count");
                return;
            }

            for (int i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i].Item;
                var isVisible = item != null && item.Definition.IsInCategory(_currentFilter);

                if (item != null) _slotsView[i].ShowItem(item, !isVisible);
                else _slotsView[i].ShowEmpty();
            }
        }
    }
}
