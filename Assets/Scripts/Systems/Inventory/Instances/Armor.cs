namespace Recalled.Systems.Inventory
{
    public class Armor : ItemInstance
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

        public Armor(ItemDefinition itemDefinition) : base(itemDefinition, 1) { }
    }
}
