using Recalled.Systems.Inventory.Stats;
using System;

namespace Recalled.Gameplay
{
    interface IStateModifierSource
    {
        event Action Changed;
        StatModifier[] Retrieve(StatDefinition statDefinition);
    }
}
