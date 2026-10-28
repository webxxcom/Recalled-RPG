using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(InventoryHolder))]
    public class ItemUser : MonoBehaviour
    {
        InventoryHolder _inventory;

        private void Awake()
        {
            _inventory = GetComponent<InventoryHolder>();
        }

        public void ConsumeItem(ItemDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException();

            if (_inventory.Inventory.Has(definition, 1) && definition.TryGetModule<Consumable>(out var module))
            {
                if (module.Consume(gameObject))
                    _inventory.Inventory.Take(definition, 1);
            }
        }
    }
}
