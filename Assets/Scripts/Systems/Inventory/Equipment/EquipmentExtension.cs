using System.Linq;

namespace Recalled.Systems.Inventory
{
    public static class EquipmentExtension
    {
        public static bool IsEquipped(this ItemInstance item, Equipment equipment)
            => equipment.Slots.Any(kv => kv.Value == item);

        internal static Equipment.EquippableItem? Equippable(this ItemInstance item, Equipment equipment)
        {
            if (!item.Definition.TryGetModule<EquippableModule>(out var module))
                return null;

            return new(item, module, item.IsEquipped(equipment));
        }
    }
}
