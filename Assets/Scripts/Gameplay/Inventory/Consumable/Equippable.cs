using Recalled.Systems.Inventory;
using System;

namespace Recalled.Gameplay
{
    [Serializable]
    public sealed class Equippable : IModule
    {
        public ItemCategory DomainCategory => ItemCategory.Equipment;
    }
}
