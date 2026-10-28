using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [Serializable]
    public class HasKeyCondition : IInteractionCondition
    {
        [SerializeField] ItemDefinition _key;

        public bool Apply(InteractionProcessor interaction)
        {
            return interaction.GetComponentInParent<InventoryHolder>().Inventory.Take(_key, 1);
        }

        public bool Check(InteractionProcessor interaction)
        {
            return interaction.GetComponentInParent<InventoryHolder>().Inventory.Has(_key, 1);
        }
    }
}
