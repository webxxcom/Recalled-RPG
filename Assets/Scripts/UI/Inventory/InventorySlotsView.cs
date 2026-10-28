using Recalled.Gameplay.Inventory;
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
        [SerializeField] InventoryAnchor _inventoryAnchor;
        [SerializeField] PrefabPopulator _populator;

        IBagView _inventoryView;
        IReadOnlyList<InventorySlot> _slotsView;
        ItemCategory _currentFilter = ItemCategory.Any;
        bool _isDirty;

        void OnEnable()
        {
            _inventoryAnchor.Provided += OnInventoryProvided;

            if (_inventoryAnchor.IsSet) _inventoryView = _inventoryAnchor.Value;
            _isDirty = true;
        }
        void OnDisable() => _inventoryAnchor.Provided -= OnInventoryProvided;

        void OnInventoryProvided(IBagView inventory)
        {
            _inventoryView = inventory;

            _inventoryView.Changed += () => _isDirty = true;
        }

        public void Filter(ItemCategory filter)
        {
            if (filter == _currentFilter)
                return;

            _currentFilter = filter;
            _isDirty = true;
        }

        public void RefreshView()
        {
            if (_inventoryView == null)
                return;

            // Lazy init
            _slotsView ??= _populator != null
                ? _populator.Populate(_inventoryView.Count).Select(go => go.GetComponent<InventorySlot>()).ToArray()
                : GetComponentsInChildren<InventorySlot>();

            if (_slotsView.Count != _inventoryView.Count)
            {
                Debug.LogError($"{name}: The {nameof(_slotsView)}'s Count doesn't match the {nameof(_inventoryView)}'s Count");
                return;
            }

            InventorySlot firstActiveSlot = default;
            for (int i = 0; i < _inventoryView.Count; i++)
            {
                var item = _inventoryView[i];
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
