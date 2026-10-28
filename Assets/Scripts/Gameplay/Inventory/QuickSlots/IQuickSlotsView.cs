using Recalled.Systems.Inventory;
using System;

namespace Recalled.Gameplay.Inventory
{
    public interface IQuickSlotsView
    {
        QuickSlots.SlotView[] Slots { get; }
        QuickSlots.SlotView this[int index] { get; }
        event Action Changed;
    }
}
    