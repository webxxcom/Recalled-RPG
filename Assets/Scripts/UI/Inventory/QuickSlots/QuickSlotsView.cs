using Recalled.Gameplay.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;

namespace Recalled.UI
{
    sealed class QuickSlotsView : MonoBehaviour
    {
        [Header("Listens")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        QuickSlotCell[] _cells;

        private void Awake() => _cells = GetComponentsInChildren<QuickSlotCell>();
        private void OnEnable()
        {
            _slotsAnchor.Provided += OnAnchorProvided;

            if (_slotsAnchor.IsSet) OnAnchorProvided(_slotsAnchor.Value);
        }
        private void OnDisable() => _slotsAnchor.Provided -= OnAnchorProvided;

        void OnAnchorProvided(IQuickSlots slots)
        {
            slots.Changed += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            var slots = _slotsAnchor.Value.Slots;
            for (int i = 0; i < _cells.Length; i++)
            {
                var slot = slots[i];

                if (!slot.Equals(default)) _cells[i].Show(slot.Definition, slot.Count);
                else _cells[i].Remove();
            }
        }
    }
}
