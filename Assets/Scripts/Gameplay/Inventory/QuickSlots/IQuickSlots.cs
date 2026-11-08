using Recalled.Systems.Inventory;

namespace Recalled.Gameplay.Inventory
{
    public interface IQuickSlots : IView
    {
        QuickSlots.SlotView[] Slots { get; }
        ItemDefinition this[int index] { get; }
        bool RequestDefinitionForSlot(ItemDefinition def, int i, out int occupiedSlot);
    }
}
