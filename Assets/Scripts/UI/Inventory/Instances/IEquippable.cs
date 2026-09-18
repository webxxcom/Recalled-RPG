
internal interface IEquippable
{
    ItemInstance Equip(InventorySO inventory)
    {
        ItemInstance equipped = this as ItemInstance;
        ItemInstance replaced = GetInTheInventory(inventory);

        SetInTheInventory(inventory, equipped);
        if (replaced != null) inventory.GeneralLoadout.Add(replaced);
        inventory.GeneralLoadout.Remove(equipped);

        return replaced;
    }
    ItemInstance Unequip(InventorySO inventory)
    {
        ItemInstance cpy = GetInTheInventory(inventory);

        SetInTheInventory(inventory, null);
        inventory.GeneralLoadout.Add(cpy);

        return cpy;
    }

    protected ItemInstance GetInTheInventory(InventorySO inventory);
    protected void SetInTheInventory(InventorySO inventory, ItemInstance val);
}
