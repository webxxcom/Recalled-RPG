
public class Boots : ItemInstance, IEquippable
{
    public new BootsDefinition Definition => base.Definition as BootsDefinition;

    public override string Description
    {
        get
        {
            return $"{Definition.Name}\n" +
                $"{Definition.Description}\n\n" +
                $"_speed Multiplier: {Definition.SpeedMultiplier}\n" +
                $"Protection: {Definition.Protection}";
        }
    }
    public Boots(ItemDefinition itemDefinition) : base(itemDefinition) { }

    ItemInstance IEquippable.GetInTheInventory(InventorySO pi) => pi.Boots;
    void IEquippable.SetInTheInventory(InventorySO pi, ItemInstance val) => pi.Boots = val as Boots;

}
