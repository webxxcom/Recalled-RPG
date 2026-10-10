using System;

namespace Recalled.Systems.Inventory
{
    public class Boots : ItemInstance
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

        internal override ItemInstance Copy()
        {
            throw new NotImplementedException();
        }
    }
}
