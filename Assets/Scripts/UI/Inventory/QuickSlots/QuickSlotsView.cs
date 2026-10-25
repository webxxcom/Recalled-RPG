using Recalled.Core;
using Recalled.Gameplay;
using Recalled.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    sealed class QuickSlotsView : MonoBehaviour
    {
        [Header("Listens")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;
        [SerializeField] VoidGameEvent _quickSlotsChanged;

        QuickSlotCell[] _cells;
        IReadOnlyList<QuickSlots.Slot> _quickSlots;

        private void Awake()
        {
            _cells = GetComponentsInChildren<QuickSlotCell>();
        }
        private void OnEnable()
        {
            _slotsAnchor.Provided += OnAnchorProvided;
            _quickSlotsChanged.AddListener(RefreshView);

            if (_slotsAnchor.IsSet) OnAnchorProvided(_slotsAnchor.Value);
        }
        private void OnDisable()
        {
            _slotsAnchor.Provided -= OnAnchorProvided;
            _quickSlotsChanged.RemoveListener(RefreshView);
        }

        void OnAnchorProvided(IReadOnlyList<QuickSlots.Slot> slots)
        {
            _quickSlots = slots;

            RefreshView();
        }

        public void RefreshView()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                var slot = _quickSlots[i];

                if (slot?.Definition != null) _cells[i].Show(slot.Definition, slot.Count);
                else _cells[i].Remove();
            }
        }
    }
}
