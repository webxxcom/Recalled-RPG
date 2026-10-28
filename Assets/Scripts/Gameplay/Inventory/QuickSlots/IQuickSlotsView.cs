using Recalled.Systems.Inventory;
using System;

namespace Recalled.Gameplay.Inventory
{
    public interface IQuickSlotsView
    {
        QuickSlots.SlotView[] Slots { get; }
        ItemDefinition this[int index] { get; }
        event Action Changed;
    }
}
    