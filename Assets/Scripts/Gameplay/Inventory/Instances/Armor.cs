public class Armor : ItemInstance, IEquippable
{
    public new ArmorDefinition Definition => base.Definition as ArmorDefinition;

    public override string Description
    {
        get
        {
            return $"{Definition.Name}\n" +
                $"{Definition.Description}\n\n" +
                $"Protection: {Definition.Protection}\n" +
                $"Weight: {Definition.Weight}";
        }
    }

    public Armor(ItemDefinition itemDefinition) : base(itemDefinition) { }

    ItemInstance IEquippable.GetInTheInventory(InventorySO pi) => pi.Armor;
    void IEquippable.SetInTheInventory(InventorySO pi, ItemInstance val) => pi.Armor = val as Armor;
}
