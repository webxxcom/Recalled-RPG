public class Sword : ItemInstance, IEquippable
{
    public new SwordDefinition Definition => base.Definition as SwordDefinition;

    public override string Description
    {
        get
        {
            return $"{Definition.Name}\n" +
                $"{Definition.Description}\n\n" +
                $"Power: {Definition.Damage}\n" +
                $"Knockback Power: {Definition.KnockbackPower}\n" +
                $"Weight: {Definition.Weight}";
        }
    }
    public Sword(ItemDefinition itemDefinition) : base(itemDefinition) { }

    ItemInstance IEquippable.GetInTheInventory(InventorySO pi) => pi.Sword;
    void IEquippable.SetInTheInventory(InventorySO pi, ItemInstance val) =>  pi.Sword = val as Sword;
}
