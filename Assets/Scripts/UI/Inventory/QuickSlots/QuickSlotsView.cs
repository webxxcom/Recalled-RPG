using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;

namespace Recalled.UI
{
    sealed class QuickSlotsView : MonoBehaviour
    {
        [Header("Listens")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;
        [SerializeField] VoidGameEvent _quickSlotsChanged;

        QuickSlotCell[] _cells;
        IQuickSlotsView _quickSlots;

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

        void OnAnchorProvided(IQuickSlotsView slots)
        {
            _quickSlots = slots;

            RefreshView();
        }

        public void RefreshView()
        {
            var slots = _quickSlots.Slots;
            for (int i = 0; i < _cells.Length; i++)
            {
                var slot = slots[i];

                if (!slot.Equals(default)) _cells[i].Show(slot.Definition, slot.Count);
                else _cells[i].Remove();
            }
        }
    }
}
