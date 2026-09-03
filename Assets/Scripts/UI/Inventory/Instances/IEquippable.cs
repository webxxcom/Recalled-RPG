internal interface IEquippable
{
    ItemInstance Equip(InventorySO inventory)
    {
        ItemInstance equipped = this as ItemInstance;
        ItemInstance replaced = GetInTheInventory(inventory);

        if (replaced != null) inventory.UncheckedAdd(replaced);
        SetInTheInventory(inventory, equipped);
        inventory.Remove(equipped);

        return replaced;
    }
    ItemInstance Unequip(InventorySO inventory)
    {
        ItemInstance cpy = GetInTheInventory(inventory);

        inventory.UncheckedAdd(cpy);
        SetInTheInventory(inventory, null);
        return cpy;
    }

    protected ItemInstance GetInTheInventory(InventorySO inventory);
    protected void SetInTheInventory(InventorySO inventory, ItemInstance val);
}
