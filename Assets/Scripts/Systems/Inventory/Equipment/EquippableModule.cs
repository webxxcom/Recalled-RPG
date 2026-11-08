using Recalled.Systems.Inventory.Stats;
using System;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [Serializable]
    public class EquippableModule : IModule
    {
        [SerializeField] SlotType _slot;
        [SerializeField] StatModifier[] _stats;

        public SlotType Slot => _slot;
        public StatModifier[] Stats => _stats;
        public ItemCategory DomainCategory => ItemCategory.Equipment;
    }
}
