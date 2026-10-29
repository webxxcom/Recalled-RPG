using Recalled.Systems.Inventory;
using System;

namespace Recalled.Gameplay.Inventory
{
    public interface IQuickSlots
    {
        QuickSlots.SlotView[] Slots { get; }
        ItemDefinition this[int index] { get; }
        event Action Changed;
        bool RequestDefinitionForSlot(ItemDefinition def, int i, out int occupiedSlot);
    }
}
    