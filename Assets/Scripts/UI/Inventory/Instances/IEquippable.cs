
internal interface IEquippable
{
    ItemInstance Equip(InventorySO inventory)
    {
        ItemInstance equipped = this as ItemInstance;
        ItemInstance replaced = GetInTheInventory(inventory);

        SetInTheInventory(inventory, equipped);
        if (replaced != null) inventory.AddItem(replaced);
        inventory.Remove(equipped);

        return replaced;
    }
    ItemInstance Unequip(InventorySO inventory)
    {
        ItemInstance cpy = GetInTheInventory(inventory);

        SetInTheInventory(inventory, null);
        inventory.AddItem(cpy);

        return cpy;
    }

    protected ItemInstance GetInTheInventory(InventorySO inventory);
    protected void SetInTheInventory(InventorySO inventory, ItemInstance val);
}
