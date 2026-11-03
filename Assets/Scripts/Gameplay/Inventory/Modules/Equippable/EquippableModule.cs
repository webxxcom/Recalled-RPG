using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    [Serializable]
    public class EquippableModule : IModule
    {
        [SerializeField] SlotType _slot;
        [SerializeField] StatModifier[] _stats;

        public SlotType Slot => _slot;
        internal StatModifier[] Stats => _stats;
        public ItemCategory DomainCategory => ItemCategory.Equipment;
    }
}
