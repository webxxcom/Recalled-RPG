
internal interface IEquippable
{
    ItemInstance Equip(InventorySO inventory)
    {
        ItemInstance equipped = this as ItemInstance;
        ItemInstance replaced = GetInTheInventory(inventory);

        SetInTheInventory(inventory, equipped);
        if (replaced != null) inventory.GeneralItems.Add(replaced);
        inventory.GeneralItems.Remove(equipped);

        return replaced;
    }
    ItemInstance Unequip(InventorySO inventory)
    {
        ItemInstance cpy = GetInTheInventory(inventory);

        SetInTheInventory(inventory, null);
        inventory.GeneralItems.Add(cpy);

        return cpy;
    }

    protected ItemInstance GetInTheInventory(InventorySO inventory);
    protected void SetInTheInventory(InventorySO inventory, ItemInstance val);
}
