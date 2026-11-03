using Recalled.Gameplay.Inventory;
using System;

namespace Recalled.Gameplay
{
    interface IStateModifierSource
    {
        StatModifier[] Retrieve(StatDefinition statDefinition);
    }
}
