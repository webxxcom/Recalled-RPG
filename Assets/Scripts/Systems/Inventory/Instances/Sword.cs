using System;

namespace Recalled.Systems.Inventory
{
    public class Sword : ItemInstance
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

        internal override ItemInstance Copy()
        {
            throw new NotImplementedException();
        }
    }
}
