using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class QuickSlotsView : UiView<IQuickSlots>
    {
        [Header("Listens")]
        [SerializeField] QuickSlotsAnchor _quickSlotsAnchor;

        QuickSlotCell[] _cells;
        protected override RuntimeAnchor<IQuickSlots> Anchor => _quickSlotsAnchor;
        private void Awake() => _cells = GetComponentsInChildren<QuickSlotCell>();

        protected override void Refresh()
        {
            var slots = _quickSlotsAnchor.Value.Slots;
            for (int i = 0; i < _cells.Length; i++)
            {
                var slot = slots[i];

                if (!slot.Equals(default)) _cells[i].Show(slot.Definition, slot.Count);
                else _cells[i].Remove();
            }
        }
    }
}
