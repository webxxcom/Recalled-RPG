using Recalled.Gameplay.Inventory;
using System;

namespace Recalled.Gameplay
{
    interface IStateModifierSource
    {
        event Action Changed;
        StatModifier[] Retrieve(StatDefinition statDefinition);
    }
}
