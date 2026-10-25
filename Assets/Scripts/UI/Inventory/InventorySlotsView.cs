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

        Bag _inventory;
        IReadOnlyList<InventorySlot> _slotsView;
        ItemCategory _currentFilter = ItemCategory.Any;
        bool _isDirty;

        private void Awake()
        {
            _inventoryVariable.Subscribe(OnInventorySet);
        }

        private void OnEnable() => _isDirty = true;

        private void OnDestroy()
        {
            _inventoryVariable.Unsubscribe(OnInventorySet);
        }

        void OnInventorySet(Systems.Inventory.Bag inventory)
        {
            if (_inventory != null) _inventory.SlotsChanged -= OnInventoryChanged;

            _inventory = inventory;
            if (_inventory != null)
            {
                _inventory.SlotsChanged += OnInventoryChanged;

                _isDirty = true;
            }
        }
        void OnInventoryChanged() => _isDirty = true;

        public void Filter(ItemCategory filter)
        {
            if (filter == _currentFilter)
                return;

            _currentFilter = filter;
            _isDirty = true;
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

            InventorySlot firstActiveSlot = default;
            for (int i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i];
                var isVisible = item != null && item.Definition.IsInCategory(_currentFilter);

                if (item != null) _slotsView[i].ShowItem(item, !isVisible);
                else _slotsView[i].ShowEmpty();

                if (firstActiveSlot == null && item != null && isVisible) firstActiveSlot = _slotsView[i];
            }

            var currentSelected = EventSystem.current.currentSelectedGameObject;
            if (currentSelected == null || (currentSelected.TryGetComponent<Selectable>(out var selectable) && !selectable.enabled))
                EventSystem.current.SetSelectedGameObject(firstActiveSlot.gameObject);
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
