internal interface IEquippable
{
    ItemInstance Equip(InventorySO inventory)
    {
        ItemInstance toSet = this as ItemInstance;
        ItemInstance replaced = GetInTheInventory(inventory);

        inventory.Add(replaced);
        SetInTheInventory(inventory, toSet);
        inventory.Remove(toSet);

        return replaced;
    }
    ItemInstance Unequip(InventorySO inventory)
    {
        ItemInstance cpy = GetInTheInventory(inventory);

        inventory.Add(cpy);
        SetInTheInventory(inventory, null);
        return cpy;
    }

    protected ItemInstance GetInTheInventory(InventorySO inventory);
    protected void SetInTheInventory(InventorySO inventory, ItemInstance val);
}
