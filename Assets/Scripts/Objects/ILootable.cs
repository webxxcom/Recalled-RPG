using UnityEngine;

public interface ILootable
{
    void LootItem(InventorySO inventory, ItemInstance item);
}
