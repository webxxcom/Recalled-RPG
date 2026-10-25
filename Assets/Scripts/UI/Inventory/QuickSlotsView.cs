using Recalled.Gameplay;
using Recalled.Systems.Inventory;
using Recalled.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    sealed class QuickSlotsView : MonoBehaviour
    {
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        InventoryCell[] _cells;

        private void Awake()
        {
            _cells = GetComponentsInChildren<InventoryCell>();
        }
        private void OnEnable()
        {
            _slotsAnchor.Provided += OnAnchorProvided;

            if (_slotsAnchor.IsSet) OnAnchorProvided(_slotsAnchor.Value);
        }
        private void OnDisable() => _slotsAnchor.Provided -= OnAnchorProvided;

        void OnAnchorProvided(IReadOnlyList<ItemInstance> slots)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                var item = slots[i];

                if (item != null) _cells[i].ShowItem(item, item.Count == 0);
                else _cells[i].ShowEmpty();
            }
        }
    }
}
